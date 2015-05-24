(function() {
    'use strict';

    angular.module('budgetApp').factory('commonService', function() {

        var getIconUrl = function (value) {

            var url = " ";

            switch (value) {
                case "All":
                    url = "../Content/images/for3.png";
                    break;
                case "Salary":
                    url = "../Content/images/payment7.png";
                    break;
                case "OtherIncome":
                    url = "../Content/images/money132.png";
                    break;
                case "Fixed":
                    url = "../Content/images/two123.png";
                    break;
                case "DebtReduction":
                    url = "../Content/images/temple18.png";
                    break;
                case "Dental":
                    url = "../Content/images/teeth.png";
                    break;
                case "Insurance":
                    url = "../Content/images/insurance.png";
                    break;
                case "Medical":
                    url = "../Content/images/medical109.png";
                    break;
                case "OtherFixed":
                    url = "../Content/images/protection3.png";
                    break;
                case "Food":
                    url = "../Content/images/fork28.png";
                    break;
                case "Groceries":
                    url = "../Content/images/shopping82.png";
                    break;
                case "OtherFood":
                    url = "../Content/images/apple55.png";
                    break;
                case "Restaurant":
                    url = "../Content/images/currency2.png";
                    break;
                case "Treats":
                    url = "../Content/images/hot51.png";
                    break;
                case "Personal":
                    url = "../Content/images/user91.png";
                    break;
                case "Appearance":
                    url = "../Content/images/suit.png";
                    break;
                case "Entertainment":
                    url = "../Content/images/videogame.png";
                    break;
                case "Gifts":
                    url = "../Content/images/gift2.png";
                    break;
                case "Hobby":
                    url = "../Content/images/letter11.png";
                    break;
                case "OtherPersonal":
                    url = "../Content/images/jumping28.png";
                    break;
                case "Phone":
                    url = "../Content/images/iphone26.png";
                    break;
                case "Subscriptions":
                    url = "../Content/images/rss22.png";
                    break;
                case "Travel":
                    url = "../Content/images/airplane73.png";
                    break;
                case "Shelter":
                    url = "../Content/images/home168.png";
                    break;
                case "Furniture":
                    url = "../Content/images/livingroom8.png";
                    break;
                case "Interior":
                    url = "../Content/images/light14.png";
                    break;
                case "Mortgage":
                    url = "../Content/images/real-estate.png";
                    break;
                case "OtherShelter":
                    url = "../Content/images/tribal.png";
                    break;
                case "Rent":
                    url = "../Content/images/house121.png";
                    break;
                case "Utilities":
                    url = "../Content/images/lightning31.png";
                    break;
                case "Transport":
                    url = "../Content/images/car95.png";
                    break;
                case "Car":
                    url = "../Content/images/car95.png";
                    break;
                case "CollectiveTransport":
                    url = "../Content/images/bus21.png";
                    break;
                case "OtherTransportation":
                    url = "../Content/images/map29.png";
                    break;
                case "Saving":
                    url = "../Content/images/piggy9.png";
                    break;
                default:
                    return this.value;
            }

            return url;
        };

        return {
            
            getIconUrl: getIconUrl

        };

    });

})();