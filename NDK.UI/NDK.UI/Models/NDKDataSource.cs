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
        where TInput : NDKRequest
        where TOptions : NDKDataSourceOptions<TOutput>
        where TOutput : NDKBaseModel
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


        public virtual async Task<NDKResponse?> FetchAsync(TInput input)
        {
            _source.Clear();
            _visibleSource.Clear();

            if (input == null) return null;

            NDKPagerModel? pager = null;
            NDKSortBy? sortBy = null;
            NDKFilterStructure? filterStructure = null;

            if (!_options.DataBaseFilter)
            {
                filterStructure = input.FilterStructure;
                input.FilterStructure = null;
            }

            if (!_options.DataBasePager)
            {
                pager = input.Pager;
                input.Pager = null;
            }

            if (!_options.DataBaseSort)
            {
                sortBy = input.SortBy;
                input.SortBy = null;
            }

            var response = await _service.FetchAsync(input);

            if (response is null)
            {
                return response;
            }

            var data = response.Result;

            if (response.HasStopFlowMessages || response?.Result?.Count == 0)
            {
                return response;
            }

            if (data is null)
            {
                return response;
            }

            foreach (var item in data)
            {
                _source.Add(item);
            }

            if (_options.DataBaseSort && _options.DataBasePager && _options.DataBaseFilter)
            {
                _visibleSource = _source;

                return response;
            };


            HandleFilterStructure(filterStructure, new NDKRef<List<TOutput>>(data));
            HandleSorter(sortBy, new NDKRef<List<TOutput>>(data));
            HandlePager(pager, new NDKRef<List<TOutput>>(data));

            foreach (var item in data)
            {
                _visibleSource.Add(item);
            }


            await Task.CompletedTask;

            return null;
        }

        private void HandlePager(NDKPagerModel? pager, NDKRef<List<TOutput>> output)
        {
            if (pager is null) return;
            if (output is null || output.Value is null) return;

            int count = pager.Page * pager.ItemsPerPage;

            output.Value = output.Value.Take(new Range(pager.Page - 1, count - 1 > output.Value.Count ? output.Value.Count - 1 : count - 1)).ToList();

        }

        private void HandleSorter(NDKSortBy? sorter, NDKRef<List<TOutput>> output)
        {
            if (sorter is null) return;
            if (output is null || output.Value is null) return;

            var data = output.Value.Order();

            if (sorter.Data is not null)
            {
                foreach (var sort in sorter.Data)
                {
                    if (string.IsNullOrWhiteSpace(sort.Field))
                        continue;

                    var property = typeof(TOutput).GetProperty(sort.Field);

                    if (property is null)
                        continue;

                    if (sort.SortType == NDKSortType.ASC)
                    {
                        data = data.ThenBy(x => property.GetValue(x));
                    }
                    else
                    {
                        data = data.ThenByDescending(x => property.GetValue(x));
                    }
                }
            }

            output.Value = data.ToList();
        }

        private void HandleFilterStructure(NDKFilterStructure? filterStructure, NDKRef<List<TOutput>> output)
        {
            if (filterStructure is null) return;
            if (output is null || output.Value is null) return;

            foreach(var fg in filterStructure.FilterGroups)
            {
                output.Value = output.Value.FindAll(x => ByGroup(x,fg));
            }

        }

        private bool ByGroup(TOutput item, NDKFilterGroup fg)
        {
            bool? result = null;

            foreach(var obj in fg.OrderList)
            {
                if (obj.Type == NDKFilterGroup.IdentifierType.Filter && obj.Value is not null)
                {
                    var model = (NDKFilter)obj.Value;

                    if (model.ConditionType == NDKConditionType.AND)
                    {
                        result = result.HasValue ? result.Value && ByFilter(item, model): ByFilter(item, model);
                    }
                    else
                    {
                        result = result.HasValue ? result.Value || ByFilter(item, model) : ByFilter(item, model);
                    }
                }
                else if (obj.Type == NDKFilterGroup.IdentifierType.FilterGroup && obj.Value is not null)
                {
                    var model = (NDKFilterGroup)obj.Value;

                    if (model.ConditionType == NDKConditionType.AND)
                    {
                        result = result.HasValue ? result.Value && ByGroup(item, model) : ByGroup(item, model);
                    }
                    else
                    {
                        result = result.HasValue ? result.Value || ByGroup(item, model) : ByGroup(item, model);
                    }
                }
            }

            
            return !result.HasValue || result.Value;
        }
        private bool ByFilter(TOutput item, NDKFilter f)
        {
            if (item is null) return false;

            if (string.IsNullOrWhiteSpace(f.PropertyName)) return true;

            PropertyInfo? propertyToCheck = typeof(TOutput).GetProperty(f.PropertyName);

            if (propertyToCheck is null) return true;

            object? value = propertyToCheck.GetValue(item);

            if (value is null)
            {
                return false;
            }

            if ((value?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false) &&
                (f.Value?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false) &&
                (f.OperatorType is not NDKOperatorType.IN and not NDKOperatorType.NOTIN))
            {
                IComparable value1 = (IComparable)value;
                IComparable value2 = (IComparable)f.Value;
                IComparable? value3 = null;

                if (f.Value2?.GetType().IsAssignableFrom(typeof(IComparable)) ?? false)
                {
                    value3 = (IComparable?)f.Value2;
                }


                return f.OperatorType switch
                {
                    NDKOperatorType.EQUAL => value1 == value2,
                    NDKOperatorType.NOTEQUAL => value1 != value2,
                    NDKOperatorType.LESSTHAN => value1.CompareTo(value2) < 0,
                    NDKOperatorType.LESSTHANOREQUAL => value1.CompareTo(value2) <= 0,
                    NDKOperatorType.GREATERTHAN => value1.CompareTo(value2) > 0,
                    NDKOperatorType.GREATERTHANOREQUAL => value1.CompareTo(value2) >= 0,
                    NDKOperatorType.BETWEEN => (value3 is null && value1.CompareTo(value2) >= 0) || (value3 is not null && value1.CompareTo(value2) >= 0 && value1.CompareTo(value3) <= 0),
                    _ => true
                };
            }
            else if (f.OperatorType is NDKOperatorType.IN or NDKOperatorType.NOTIN)
            {

                if (f.Value?.GetType().IsAssignableFrom(typeof(IEnumerable<object>)) ?? false)
                {
                    var list = ((IEnumerable<object>)f.Value).ToList();

                    if (list.GetType().GetEnumUnderlyingType() == value?.GetType())
                    {
                        return (f.OperatorType == NDKOperatorType.IN && list.Contains(value)) || (f.OperatorType == NDKOperatorType.NOTIN && !list.Contains(value));
                    }
                }
            }
            else if (f.OperatorType is NDKOperatorType.STARTSWITH or NDKOperatorType.ENDSWITH or NDKOperatorType.CONTAINS)
            {
                if (f.Value?.GetType().IsAssignableFrom(typeof(string)) ?? false)
                {
                    var value1 = (string)value!;
                    var value2 = (string)f.Value;

                    return f.OperatorType switch
                    {
                        NDKOperatorType.STARTSWITH => value1.StartsWith(value2),
                        NDKOperatorType.ENDSWITH => value1.EndsWith(value2),
                        NDKOperatorType.CONTAINS => value1.Contains(value2),
                        _ => true
                    };
                }
            }

            return true;
        }
       

    }
}
