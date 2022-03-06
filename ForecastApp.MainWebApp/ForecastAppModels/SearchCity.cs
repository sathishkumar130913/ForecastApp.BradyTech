using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastApp.ForecastAppModels
{
    public class SearchCity
    {
        // Annotations required to validate input data in our model.
        [Required(ErrorMessage = "You must enter a city/area name!")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Only text allowed")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Enter a city/area name greater than 2 and lesser than 20 characters!")]
        [Display(Name = "City/Area Name")]
        public string CityName { get; set; }
    }
}