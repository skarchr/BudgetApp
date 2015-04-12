using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
    }

    public enum Category
    {
        #region Food

        Groceries,
        Restaurant,
        Treats,
        OtherFood,

        #endregion

        #region Shelter

        Furniture,
        Interior,
        Rent,
        Mortgage,
        Utilities,
        RenovationRepairsMaintenance,
        OtherShelter,

        #endregion

        #region Transportation

        Car,
        Fuel,
        Repairs,
        Maintenance,
        Parts,
        Plane,
        CollectiveTransport,
        OtherTransportation,

        #endregion

        #region Personal
        
        Phone,
        Hobby,
        Vacation,
        Clothes,
        Entertainment,
        Subscriptions,
        OtherPersonal,

        #endregion

        #region Fixed
        
        Insurance,
        Medical,
        Dental,
        DebtReduction,
        Other

        #endregion


    }
}