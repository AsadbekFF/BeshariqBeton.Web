using BeshariqBeton.Common.Entities.Base;
using BeshariqBeton.Common.Exceptions;
using BeshariqBeton.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;
using BeshariqBeton.Common.Interfaces;
using BeshariqBeton.Common.Utilities;

namespace BeshariqBeton.BLL.Base
{
    public class BaseService<T, TId, TContext> where T : BaseEntity<TId>, new() where TContext : DbContext
    {
        protected readonly TContext Context;
        protected virtual string[] IncludedPropertiesForExport => new string[0];

        public BaseService(TContext context)
        {
            Context = context;
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(TId id)
        {
            return await GetByIdAsync(id, null);
        }

        protected virtual async Task<T> GetByIdAsync(TId id, string[] includedProperties)
        {
            var query = Context.Set<T>().AsTracking().AsQueryable();

            if (includedProperties != null)
                foreach (var includeProperty in includedProperties)
                    query = query.Include(includeProperty);

            var entity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));

            return entity;
        }

        public virtual async Task<T> GetByIdNotTrackingAsync(TId id)
        {
            return await GetByIdNotTrackingAsync(id, null);
        }

        protected virtual async Task<T> GetByIdNotTrackingAsync(TId id, string[] includedProperties)
        {
            var query = Context.Set<T>().AsQueryable();

            if (includedProperties != null)
                foreach (var includeProperty in includedProperties)
                    query = query.Include(includeProperty);

            var entity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));

            return entity;
        }

        protected virtual async Task<T> GetByIdNotTrackingAndLoadPropertiesAfterAsync(TId id, string[] includedProperties)
        {
            var entity = await Context.Set<T>().AsTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));

            if (entity == null)
                throw new NotFoundException();

            await LoadRelatedProperties(entity, includedProperties);

            return entity;
        }

        protected async Task LoadRelatedProperties(object entity, string[] properties)
        {
            if (properties != null)
            {
                Context.Attach(entity);
                var entry = Context.Entry(entity);

                try
                {
                    foreach (var includeProperty in properties)
                    {
                        var currentEntry = entry;
                        var parts = includeProperty.Split('.');

                        foreach (var part in parts)
                        {
                            // Property not found
                            if (currentEntry.Members.All(m => m.Metadata.Name != part))
                                break;

                            var member = currentEntry.Member(part);

                            if (member is CollectionEntry collectionEntry)
                            {
                                await collectionEntry.LoadAsync();
                                break; // Do not load child of collections
                            }

                            if (member is ReferenceEntry referenceMember)
                            {
                                await referenceMember.LoadAsync();
                                currentEntry = referenceMember.TargetEntry;
                            }

                            if (currentEntry == null)
                                break;

                        }
                    }
                }
                // Detach at the end in order not to keep tracking
                finally
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            await DoValidation(entity);
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await DoValidation(entity);

            var entry = Context.Entry(entity);
            entry.State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual Task DeleteByIdAsync(TId id)
        {
            var entity = new T
            {
                Id = id
            };

            var entry = Context.Entry(entity);
            entry.State = EntityState.Deleted;

            return Context.SaveChangesAsync();
        }

        public virtual Task<Items<T>> FilterAsync(string sort, string order, int limit, int offset)
        {
            return FilterAsync(sort, order, limit, offset, null, null, null);
        }

        protected Task<Items<T>> FilterAsync(string sort, string order, int limit, int offset, Expression<Func<T, bool>> filterExpression)
        {
            return FilterAsync(sort, order, limit, offset, filterExpression, null, null);
        }

        protected Task<Items<T>> FilterAsync(string sort, string order, int limit, int offset, string[] includedProperties)
        {
            return FilterAsync(sort, order, limit, offset, null, includedProperties, null);
        }

        protected Task<Items<T>> FilterAsync(string sort, string order, int limit, int offset, Expression<Func<T, T>> projectionExpression)
        {
            return FilterAsync(sort, order, limit, offset, null, null, projectionExpression);
        }

        protected async Task<Items<T>> FilterAsync(string sort, string order, int limit, int offset, Expression<Func<T, bool>> filterExpression, string[] includedProperties, Expression<Func<T, T>> projectionExpression)
        {
            var items = Context.Set<T>().AsQueryable();

            // Included properties
            if (includedProperties != null)
                foreach (var includeProperty in includedProperties)
                    items = items.Include(includeProperty);

            if (filterExpression != null)
                items = items.Where(filterExpression);

            // Order
            if (!string.IsNullOrEmpty(sort))
                items = items.OrderBy(sort + " " + order);

            // Total items
            var total = await items.CountAsync();

            // Pagination
            if (limit > 0)
                items = items.Skip(offset).Take(limit);

            // Projection
            if (projectionExpression != null)
                items = items.Select(projectionExpression);

            // Pagination
            return new Items<T>
            {
                Rows = await items.ToListAsync(),
                Total = total
            };
        }

        public async Task CheckDuplicatesAsync(T entity, List<ValidationResult> validationResults, string errorField,
            string errorMessage, Expression<Func<T, bool>> predicate)
        {
            var duplicateQuery = Context.Set<T>().AsQueryable();

            // Check properties
            duplicateQuery = duplicateQuery.Where(predicate).AsQueryable();

            // If update
            var newId = default(TId);
            if (!EqualityComparer<TId>.Default.Equals(entity.Id, newId))
                duplicateQuery = duplicateQuery.Where($"Id <> {entity.Id}");

            if (await duplicateQuery.AnyAsync())
                validationResults.AddError(errorField, errorMessage);
        }

        public virtual async Task<string> GetNextVersionNameByIdAsync(TId id)
        {
            var name = ((INameEntity)await GetByIdNotTrackingAsync(id, null)).Name;

            // If the name already contain version - remove it
            var match = BaseServiceUtilities.VersionRegex.Match(name);
            if (match.Success)
                name = match.Groups[1].Value;

            // Get names from DB which can be the versions of entity name
            var possibleNames = await Context.Set<T>().Where("Name.StartsWith(@0)", name + "-V").Select<string>("Name").ToListAsync();
            var latestVersion = BaseServiceUtilities.GetVersionFromNames(possibleNames);
            return $"{name}-V{latestVersion + 1}";
        }

        protected virtual T GetEntity(Stream stream)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var entity = serializer.Deserialize<T>(jsonTextReader);

            return entity;
        }

        protected virtual async Task<IEnumerable<ValidationResult>> ValidateAsync(T entity)
        {
            return null;
        }

        protected async Task DoValidation(T entity)
        {
            var results = await ValidateAsync(entity);

            if (results != null && results.Any())
                throw new ValidationResultException(results);
        }
    }

    // Internal utilities, in order not to store static data in generic class
    internal static class BaseServiceUtilities
    {
        internal static readonly Regex VersionRegex = new Regex("(.*)-V([0-9]+)", RegexOptions.Compiled);

        /// <summary>
        /// Get last version from collection of entity names like Schema-V1, Schema-V2.
        /// </summary>
        /// <param name="names">Collection of names.</param>
        /// <returns>Last version number.</returns>
        internal static int GetVersionFromNames(IEnumerable<string> names)
        {
            var latestVersion = 1;

            foreach (var possibleName in names)
            {
                var nameMatch = VersionRegex.Match(possibleName);

                if (nameMatch.Success)
                {
                    var versionNumber = int.Parse(nameMatch.Groups[2].Value);
                    if (versionNumber > latestVersion)
                        latestVersion = versionNumber;
                }
            }

            return latestVersion;
        }
    }
}
