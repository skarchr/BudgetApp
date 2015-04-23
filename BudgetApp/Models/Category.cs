using System.ComponentModel;

namespace BudgetApp.Models
{
    public enum Category
    {
        #region Income
        
        Salary,

        [Description("Other income")]
        OtherIncome,
        
        #endregion

        #region Food

        Groceries,
        Restaurant,
        Treats,

        [Description("Other food")]
        OtherFood,

        #endregion

        #region Shelter

        Furniture,
        Interior,
        Rent,
        Mortgage,
        Utilities,

        [Description("Other shelter")]
        OtherShelter,

        #endregion

        #region Transportation

        Car,
        [Description("Collective transport")]
        CollectiveTransport,

        [Description("Other transport")]
        OtherTransportation,

        #endregion

        #region Personal
        
        Phone,
        Hobby,
        Travel,
        Entertainment,
        Subscriptions,
        Gifts,
        Appearance,

        [Description("Other personal")]
        OtherPersonal,

        #endregion

        #region Fixed
        
        Insurance,
        Medical,
        Dental,

        [Description("Debt reduction")]
        DebtReduction,

        [Description("Other fixed")]
        OtherFixed

        #endregion

        ,
        Saving
    }
}