(function() {
    'use strict';

    angular.module('budgetApp').service('sharedProperties',[function() {
        var selected = 'charts';

        return {

            getSelected: function () {

                return selected;
            },
            setSelected: function (name) {

                selected = name;
            }
        };
    }]);


    angular.module('budgetApp').factory('commonService', [function() {

        var getIconUrl = function (value) {

            var imageName = "";

            switch (value) {
                case "Atm":
                    imageName = "atm2.png";
                    break;
                case "ATM":
                    imageName = "atm2.png";
                    break;
                case "Expenses":
                    imageName = "hand132.png";
                    break;
                case "Google":
                    imageName = "googleplus.png";
                    break;
                case "Twitter":
                    imageName = "twitter.png";
                    break;
                case "Facebook":
                    imageName = "facebook.png";
                    break;
                case "All":
                    imageName = "for3.png";
                    break;
                case "Salary":
                    imageName = "payment7.png";
                    break;
                case "Income":
                    imageName = "money132.png";
                    break;
                case "OtherIncome":
                    imageName = "money132.png";
                    break;
                case "Fixed":
                    imageName = "two123.png";
                    break;
                case "DebtReduction":
                    imageName = "temple18.png";
                    break;
                case "Dental":
                    imageName = "teeth.png";
                    break;
                case "Insurance":
                    imageName = "insurance.png";
                    break;
                case "Medical":
                    imageName = "medical109.png";
                    break;
                case "OtherFixed":
                    imageName = "protection3.png";
                    break;
                case "Food":
                    imageName = "fork28.png";
                    break;
                case "Groceries":
                    imageName = "shopping82.png";
                    break;
                case "OtherFood":
                    imageName = "apple55.png";
                    break;
                case "Restaurant":
                    imageName = "currency2.png";
                    break;
                case "Treats":
                    imageName = "hot51.png";
                    break;
                case "Personal":
                    imageName = "user91.png";
                    break;
                case "Appearance":
                    imageName = "suit.png";
                    break;
                case "Entertainment":
                    imageName = "videogame.png";
                    break;
                case "Gifts":
                    imageName = "gift2.png";
                    break;
                case "Hobby":
                    imageName = "letter11.png";
                    break;
                case "OtherPersonal":
                    imageName = "jumping28.png";
                    break;
                case "Phone":
                    imageName = "iphone26.png";
                    break;
                case "Subscriptions":
                    imageName = "rss22.png";
                    break;
                case "Travel":
                    imageName = "airplane73.png";
                    break;
                case "Shelter":
                    imageName = "home168.png";
                    break;
                case "Furniture":
                    imageName = "livingroom8.png";
                    break;
                case "Interior":
                    imageName = "light14.png";
                    break;
                case "Mortgage":
                    imageName = "real-estate.png";
                    break;
                case "OtherShelter":
                    imageName = "tribal.png";
                    break;
                case "Rent":
                    imageName = "house121.png";
                    break;
                case "Utilities":
                    imageName = "lightning31.png";
                    break;
                case "Transport":
                    imageName = "car95.png";
                    break;
                case "Car":
                    imageName = "car95.png";
                    break;
                case "CollectiveTransport":
                    imageName = "bus21.png";
                    break;
                case "OtherTransportation":
                    imageName = "map29.png";
                    break;
                case "Saving":
                    imageName = "piggy9.png";
                    break;
                case "Workout":
                    imageName = "upper20.png";
                    break;
                case "Fuel":
                    imageName = "fuel4.png";
                    break;
                default:
                    imageName = "question30.png";
            }

            return "../../Content/images/" + imageName;

        };

        return {
            
            getIconUrl: getIconUrl

        };

    }]);

})();