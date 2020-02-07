using BranchSdk.Enum;
using BranchSdk.Net;
using BranchSdk.Net.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk {
    public class BranchEvent {
        private readonly string eventName;
        private readonly bool isStandartEvent;
        private readonly Dictionary<string, object> topLevelProperties;
        private readonly Dictionary<string, object> standartProperties;
        private readonly Dictionary<string, object> customProperties;
        private readonly List<BranchUniversalObject> buoList;

        public BranchEvent(BranchStandartEvent branchStandartEvent) {
            topLevelProperties = new Dictionary<string, object>();
            standartProperties = new Dictionary<string, object>();
            customProperties = new Dictionary<string, object>();
            this.eventName = BranchEnumUtils.GetEventName(branchStandartEvent);
            this.isStandartEvent = true;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent(string eventName) {
            topLevelProperties = new Dictionary<string, object>();
            standartProperties = new Dictionary<string, object>();
            customProperties = new Dictionary<string, object>();
            this.eventName = eventName;
            this.isStandartEvent = false;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent(string eventName, bool isStandartEvent) {
            topLevelProperties = new Dictionary<string, object>();
            standartProperties = new Dictionary<string, object>();
            customProperties = new Dictionary<string, object>();
            this.eventName = eventName;
            this.isStandartEvent = isStandartEvent;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent SetCustomerEventAlias(string customerEventAlias)
        {
            return AddTopLevelProperty(BranchEnumUtils.GetKey(BranchJsonKey.CustomerEventAlias), customerEventAlias);
        }

        public BranchEvent SetTransactionID(string transactionID) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.TransactionID), transactionID);
        }

        public BranchEvent SetCurrency(BranchCurrencyType currency) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Currency), currency.ToString());
        }

        public BranchEvent SetRevenue(double revenue) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Revenue), revenue.ToString());
        }

        public BranchEvent SetShipping(double shipping) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Shipping), shipping.ToString());
        }

        public BranchEvent SetTax(double tax) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Tax), tax.ToString());
        }

        public BranchEvent SetCoupon(string coupon) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Coupon), coupon);
        }

        public BranchEvent SetAffiliation(string affiliation) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Affiliation), affiliation);
        }

        public BranchEvent SetDescription(string description) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.Description), description);
        }

        public BranchEvent SetSearchQuery(string searchQuery) {
            return AddStandardProperty(BranchEnumUtils.GetKey(BranchJsonKey.SearchQuery), searchQuery);
        }

        private BranchEvent AddStandardProperty(string propertyName, string propertyValue) {
            if (propertyValue != null) {
                try {
                    this.standartProperties.Add(propertyName, propertyValue);
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            } else {
                this.standartProperties.Remove(propertyName);
            }
            return this;
        }

        private BranchEvent AddTopLevelProperty(string propertyName, string propertyValue)
        {
            if (!this.topLevelProperties.ContainsKey(propertyName)) {
                this.topLevelProperties.Add(propertyName, propertyValue);
            } else {
                this.topLevelProperties.Remove(propertyName);
            }
            return this;
        }

        public BranchEvent AddCustomDataProperty(string propertyName, string propertyValue) {
            try {
                this.customProperties.Add(propertyName, propertyValue);
            } catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
            return this;
        }

        public BranchEvent AddContentItems(params BranchUniversalObject[] contentItems) {
            buoList.AddRange(contentItems);
            return this;
        }

        public BranchEvent AddContentItems(List<BranchUniversalObject> contentItems) {
            buoList.AddRange(contentItems);
            return this;
        }

        public void LogEvent() {
            string requestPath = isStandartEvent ? BranchEnumUtils.GetPath(RequestPath.TrackStandardEvent) : BranchEnumUtils.GetPath(RequestPath.TrackCustomEvent);
            BranchServerRequest request = new BranchServerLogEvent(requestPath, eventName, isStandartEvent, buoList, topLevelProperties, standartProperties, customProperties);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }
    }
}
