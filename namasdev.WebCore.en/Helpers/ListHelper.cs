using System.Collections;
using System.Globalization;

using Microsoft.AspNetCore.Mvc.Rendering;

using namasdev.Core.Types;

namespace namasdev.WebCore.Helpers
{
    public class ListHelper
    {
        public static SelectList GetRolesSelectList(IEnumerable<string> roles,
            string? selectedValue = null)
        {
            return CreateSelectListFromItems(
                GetRolesSelectListItems(roles),
                selectedValue: selectedValue);
        }

        public static SelectList GetRolesSelectListMultiple(IEnumerable<string> roles,
            IEnumerable<string>? selectedValues = null)
        {
            return CreateSelectListFromItems(GetRolesSelectListItems(roles, selectedValues));
        }

        public static IEnumerable<SelectListItem> GetRolesSelectListItems(IEnumerable<string> roles,
            IEnumerable<string>? selectedValues = null)
        {
            return roles
                .Select(r => new SelectListItem
                {
                    Text = r,
                    Value = r,
                    Selected = selectedValues != null && selectedValues.Contains(r)
                })
                .ToArray();
        }

        public static SelectList GetYesNoSelectList(
            bool? selectedValue = null)
        {
            return CreateSelectListFromItems(new List<SelectListItem>
                {
                    new SelectListItem { Text = Formatter.YesNo(true), Value = true.ToString() },
                    new SelectListItem { Text = Formatter.YesNo(false), Value = false.ToString() },
                },
                selectedValue?.ToString());
        }

        public static SelectList GetYearsUpToCurrentSelectList(int fromYear, int toYear)
        {
            return GetYearsSelectList(fromYear,
                toYear: DateTime.Today.Year);
        }

        public static SelectList GetYearsSelectList(int fromYear, int toYear)
        {
            var items = new List<int>();
            for (int year = fromYear; year <= toYear; year++)
            {
                items.Add(year);
            }
            return CreateSelectListFromList(items);
        }

        public static SelectList GetYearsSelectList(IEnumerable<short> years)
        {
            return CreateSelectListFromList(years);
        }

        public static SelectList GetMonthsOfYearSelectList(IEnumerable<MonthOfYear> monthsOfYear)
        {
            return CreateSelectListFromList(
                monthsOfYear,
                m => new SelectListItem { Value = m.YearAndMonth, Text = m.ToString() });
        }

        public static SelectList GetMonthSelectList()
        {
            var nombreMeses = DateTimeFormatInfo.CurrentInfo!.MonthNames
                .Take(12)
                .Select((name, index) => new SelectListItem
                {
                    Value = (index + 1).ToString(),
                    Text = name,
                })
                .ToList();

            return CreateSelectListFromItems(nombreMeses);
        }

        public static SelectList GetMonthsSelectList(IEnumerable<short> months)
        {
            return CreateSelectListFromList(
                months,
                a => new SelectListItem
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(a),
                    Value = a.ToString()
                });
        }

        public static SelectList GetQuartersSelectList()
        {
            return CreateSelectListFromItems(Enumerable.Range(1, 4)
                .Select(i => new SelectListItem { Text = $"Q{i}", Value = i.ToString() }));
        }

        public static IEnumerable<SelectListItem> GetQuarterOfYearMonthsSelectListItems(
            QuarterOfYear quarterOfYear,
            IEnumerable<short>? selectedValues = null)
        {
            return quarterOfYear
                .GetMonthsInQuarter()
                .Select(t => new SelectListItem
                {
                    Text = t.MonthName.Capitalize(),
                    Value = t.Month.ToString(),
                    Selected = selectedValues != null && selectedValues.Contains(t.Month)
                })
                .ToList();
        }

        public static SelectList GetWeekDaysSelectList()
        {
            var items = Enumerable.Range(0, 7)
                .Select(day => new SelectListItem
                {
                    Text = Formatter.Day(day),
                    Value = day.ToString(),
                });
            return CreateSelectListFromItems(items);
        }

        public static IEnumerable<SelectListItem> GetTimesSelectListItems(
            TimeSpan? interval = null,
            IEnumerable<TimeSpan>? selectedValues = null)
        {
            interval = interval ?? TimeSpan.FromMinutes(30);

            var items = new List<SelectListItem>();
            var time = new TimeSpan();
            while (true)
            {
                items.Add(new SelectListItem
                {
                    Text = Formatter.Time(time),
                    Value = time.ToString(),
                    Selected = selectedValues != null && selectedValues.Contains(time)
                });

                time = time.Add(interval.Value);

                if (time.Days > 0)
                {
                    break;
                }
            }
            return CreateSelectListFromItems(items);
        }

        public static SelectList GetEmptySelectList()
        {
            return CreateSelectListFromItems(Array.Empty<SelectListItem>());
        }

        public static SelectList CreateSelectListFromList<T>(IEnumerable<T> items,
            Func<T, SelectListItem> selector)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectList(items.Select(selector), "Value", "Text");
        }

        public static SelectList CreateSelectListFromList(IEnumerable items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return new SelectList(items);
        }

        public static SelectList CreateSelectListFromItems(IEnumerable<SelectListItem> items,
            string? selectedValue = null)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return new SelectList(items, "Value", "Text", selectedValue);
        }
    }
}
