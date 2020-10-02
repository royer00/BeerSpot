using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BeerSpot.DATA.EF
{
    public class BeerMetaData
    {
        #region Beer
        [Display(Name = "Beer")]
        [Required(ErrorMessage = "*Name is required")]
        [StringLength(30, ErrorMessage = "*Cannot be more than 30 characters")]
        public string Name { get; set; }


        [Display(Name = "Price per Beer")]
        [Required(ErrorMessage = "Price per Beer is required")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Range(0, 1000.00, ErrorMessage = "Price per Beer must be greater than 0 and less than 1000")]
        public decimal PricePerBeer { get; set; }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString ="{0:0.00}p")]
        public Nullable<decimal> ABV { get; set; }
        
        [Display(Name="Image")]
        public string BeerImage { get; set; }

        [Display(Name="Seasonal")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public Nullable<bool> IsSeasonal { get; set; }
        
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [DisplayFormat(NullDisplayText ="N/A")]
        public string Description { get; set; }

        [Display(Name="Style")]
        public int StyleID { get; set; }
        
        

    }
    

    [MetadataType(typeof(BeerMetaData))]
    public partial class Beer
    {

    }
    #endregion
    #region Brewers
    public class BrewerMetaData
    {
        [Display(Name = "Brewery")]
        [Required(ErrorMessage ="*Name is required")]
        [StringLength(30,ErrorMessage ="*Cannot be more than 30 characters")]
        public string Name { get; set; }
        [StringLength(50, ErrorMessage ="*Cannot be more than 50 characters")]
        public string Address { get; set; }
        [StringLength(15, ErrorMessage ="*Cannot be more than 15 characters")]
        public string City { get; set; }
        [StringLength(2, ErrorMessage ="*Must be 2 characters")]
        public string State { get; set; }
        [Display(Name="Zip")]
        [StringLength(10, ErrorMessage ="*Cannot be more than 10 characters")]
        public string PostalCode { get; set; }
        [StringLength(30, ErrorMessage ="*Cannot be more than 30 characters")]
        public string Country { get; set; }
        [StringLength(24, ErrorMessage ="*Cannot be more than 24 characters")]
        public string Phone { get; set; }
        
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(BrewerMetaData))]
    public partial class Brewer
    {
        
    }
    #endregion

    #region Styles
    public class StyleMetadata
    {
        [Display(Name ="Style")]
        [StringLength(30,ErrorMessage ="*Style Name is required")]
        public string StyleName { get; set; }
    }
    [MetadataType(typeof(StyleMetadata))]
    public partial class Style
    {

    }
    #endregion


    #region Container
    public class ContainerMetaData
    {
        [Required(ErrorMessage ="*Type of Container is required")]
        [Display(Name ="Type of Container")]
        [StringLength(30, ErrorMessage ="*Must be less than 30 characters")]
        public string Type { get; set; }
    }
    #endregion

    #region BeersContainers
    public class BeersContainerMetadata
    {
        [Display(Name = "Available In")]
        public int BeersContainersID { get; set; }
        [Required(ErrorMessage ="*Beer Id is required")]
        public int BeerID { get; set; }
        [Required(ErrorMessage ="*Container Id is required")]
        public int ContainerID { get; set; }

        public virtual Beer Beer { get; set; }
        public virtual Container Container { get; set; }
    }
    #endregion

   
}
