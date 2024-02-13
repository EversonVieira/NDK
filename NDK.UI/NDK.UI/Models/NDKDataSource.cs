using NDK.Core.Models;
using NDK.UI.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Models
{
    public class NDKDataSource<TInput, TOutput, TOptions>
        where TInput : NdkRequest
        where TOptions : NDKDataSourceOptions<TOutput>
        where TOutput : NdkBaseModel
    {
        private ObservableCollection<TOutput> _source;
        private ObservableCollection<TOutput> _visibleSource;
        private TOptions _options;
        private INDKDataSourceService<TOutput, TInput> _service;
        public ObservableCollection<TOutput> DataSource => _visibleSource;
        public ObservableCollection<TOutput> SelectedData { get; set; }

        public NDKDataSource(INDKDataSourceService<TOutput, TInput> service, TOptions options)
        {
            _source = new ObservableCollection<TOutput>();
            _visibleSource = new ObservableCollection<TOutput>();
            _service = service;
            _options = options;
            SelectedData = new ObservableCollection<TOutput>();
        }


        public virtual async Task FetchAsync(TInput input)
        {
            if (input == null) return;

            NdkPaging? paging = null;
            List<NdkOrderItem>? orderBy = null;
            List<NdkFilterGroup>? filterGroups = null;


            bool isDbOperation = _options.DataBaseFiltering && _options.DataBaseOrdering && _options.DataBasePagination;
            if (!_options.DataBaseOrdering)
            {
                filterGroups = input.FiltersGroups.ToList();
                input.ClearFilters();
            }

            if (!_options.DataBaseFiltering)
            {
                orderBy = input.OrderBy;
                input.ClearOrderBy();
            }

            if (!_options.DataBasePagination)
            {
                paging = input.Paging;
                input.ClearPaging();
            }

            var response = await _service.FetchAsync(input);

            var result = response.Result;

            _source.Clear();
            _visibleSource.Clear();
            if (result is null)
            {
                return;
            }

            for (int i = 0; i < result.Count; i++)
            {
                _source.Add(result[i]);
                if (isDbOperation)
                {
                    _visibleSource.Add(result[i]);
                }
            }

            if (!isDbOperation)
            {
                if (!_options.DataBaseFiltering) HandleFilterGroups(result, filterGroups);
                if (!_options.DataBasePagination) HandlePaging(result, paging);
                if (!_options.DataBaseOrdering) HandleOrderBy(result, orderBy);

                for (int i = 0; i < result.Count; i++)
                {
                    _visibleSource.Add(result[i]);
                }
            }

            if (_options.RemoveSelectedDataFunction is not null)
            {
                var data = _visibleSource.Where(x => _options.RemoveSelectedDataFunction(x));
                _visibleSource.Clear();
                foreach (var item in data)
                {
                    _visibleSource.Add(item);
                }

            }
        }

        private void HandleFilterGroups(List<TOutput> result, List<NdkFilterGroup>? filterGroups)
        {
            if (filterGroups is null) return;

            var tempResult = new List<TOutput>();
            tempResult.AddRange(result);

            filterGroups.ForEach(group =>
            {
                if (group.Filters is not null)
                {
                    group.Filters.ForEach(f =>
                    {

                        tempResult = tempResult.FindAll(x => ByOperationType(x, f));
                    });
                }
            });

            result.Clear();
            result.AddRange(tempResult);
        }

        private bool ByOperationType(TOutput item, NdkFilter f)
        {
            if (item is null) return false;

            if (string.IsNullOrWhiteSpace(f.PropertyName)) return true;

            PropertyInfo? propertyToCheck = typeof(TOutput).GetProperty(f.PropertyName);

            if (propertyToCheck is null) return true;

            object? value = propertyToCheck.GetValue(item);


            if ((value?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false) &&
                (f.Value?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false) &&
                (f.NdkOperatorType is not NdkOperatorType.IN and not NdkOperatorType.NOTIN))
            {
                IComparable value1 = (IComparable)value;
                IComparable value2 = (IComparable)f.Value;
                IComparable? value3 = null;

                if (f.Value2?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false)
                {
                    value3 = (IComparable?)f.Value2;
                }


                return f.NdkOperatorType switch
                {
                    NdkOperatorType.EQUAL => value1 == value2,
                    NdkOperatorType.NOTEQUAL => value1 != value2,
                    NdkOperatorType.LESSTHAN => value1.CompareTo(value2) < 0,
                    NdkOperatorType.LESSTHANOREQUAL => value1.CompareTo(value2) <= 0,
                    NdkOperatorType.GREATERTHAN => value1.CompareTo(value2) > 0,
                    NdkOperatorType.GREATERTHANOREQUAL => value1.CompareTo(value2) >= 0,
                    NdkOperatorType.BETWEEN => (value3 is null && value1.CompareTo(value2) >= 0) || (value3 is not null && value1.CompareTo(value2) >= 0 && value1.CompareTo(value3) <= 0),
                    _ => true
                };
            }
            else if (f.NdkOperatorType is NdkOperatorType.IN or NdkOperatorType.NOTIN)
            {

                if (f.Value?.GetType().IsAssignableFrom(typeof(IEnumerable<object>)) ?? false)
                {
                    var list = ((IEnumerable<object>)f.Value).ToList();

                    if (list.GetType().GetEnumUnderlyingType() == value?.GetType())
                    {
                        return (f.NdkOperatorType == NdkOperatorType.IN && list.Contains(value)) || (f.NdkOperatorType == NdkOperatorType.NOTIN && list.Contains(value));
                    }
                }
            }

            return true;
        }

        private void HandlePaging(List<TOutput> result, NdkPaging? paging)
        {

            if (paging is null)
            {
                foreach (var item in _source)
                {
                    _visibleSource.Add(item);
                }
            }
            else
            {
                _visibleSource.Clear();
                for (int x = (paging.Page - 1); x < (paging.Page * paging.ItemsPerPage); x++)
                {
                    _visibleSource.Add(result[x]);
                }
            }
        }

        private void HandleOrderBy(List<TOutput> result, List<NdkOrderItem>? orderList)
        {
            if (orderList is null) return;

            var tempResult = result.Order();

            bool firstOperation = true;

            foreach (var orderBy in orderList)
            {
                if (orderBy is null || string.IsNullOrWhiteSpace(orderBy.Column)) continue;

                var property = typeof(TOutput).GetProperty(orderBy.Column);

                if (property is null) continue;

                if (firstOperation)
                {
                    tempResult = orderBy.OrderType == NdkOrderType.ASC ? tempResult.OrderBy(x => property.GetValue(x)) : tempResult.OrderByDescending(x => property.GetValue(x));
                }
                else
                {
                    tempResult = orderBy.OrderType == NdkOrderType.ASC ? tempResult.ThenBy(x => property.GetValue(x)) : tempResult.ThenByDescending(x => property.GetValue(x));

                }

            }

            result.Clear();

            result.AddRange(tempResult);
        }


    }
}
