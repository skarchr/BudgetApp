using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public Category? Category { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
    }

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
        Fuel,
        Repairs,
        Maintenance,
        Parts,

        [Description("Parking fee")]
        ParkingFees,

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


    }
}