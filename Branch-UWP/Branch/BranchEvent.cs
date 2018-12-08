using BranchSdk.Enum;
using BranchSdk.Net;
using BranchSdk.Net.Requests;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk {
    public class BranchEvent {
        private readonly string eventName;
        private readonly bool isStandartEvent;
        private readonly JsonObject standartProperties;
        private readonly JsonObject customProperties;
        private readonly List<BranchUniversalObject> buoList;

        public BranchEvent(BranchStandartEvent branchStandartEvent) {
            standartProperties = new JsonObject();
            customProperties = new JsonObject();
            this.eventName = branchStandartEvent.GetEventName();
            this.isStandartEvent = true;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent(string eventName) {
            standartProperties = new JsonObject();
            customProperties = new JsonObject();
            this.eventName = eventName;
            this.isStandartEvent = false;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent(string eventName, bool isStandartEvent) {
            standartProperties = new JsonObject();
            customProperties = new JsonObject();
            this.eventName = eventName;
            this.isStandartEvent = isStandartEvent;
            buoList = new List<BranchUniversalObject>();
        }

        public BranchEvent SetTransactionID(string transactionID) {
            return AddStandardProperty(BranchJsonKey.TransactionID.GetKey(), transactionID);
        }

        public BranchEvent SetCurrency(BranchCurrencyType currency) {
            return AddStandardProperty(BranchJsonKey.Currency.GetKey(), currency.ToString());
        }

        public BranchEvent SetRevenue(double revenue) {
            return AddStandardProperty(BranchJsonKey.Revenue.GetKey(), revenue.ToString());
        }

        public BranchEvent SetShipping(double shipping) {
            return AddStandardProperty(BranchJsonKey.Shipping.GetKey(), shipping.ToString());
        }

        public BranchEvent SetTax(double tax) {
            return AddStandardProperty(BranchJsonKey.Tax.GetKey(), tax.ToString());
        }

        public BranchEvent SetCoupon(string coupon) {
            return AddStandardProperty(BranchJsonKey.Coupon.GetKey(), coupon);
        }

        public BranchEvent SetAffiliation(string affiliation) {
            return AddStandardProperty(BranchJsonKey.Affiliation.GetKey(), affiliation);
        }

        public BranchEvent SetDescription(string description) {
            return AddStandardProperty(BranchJsonKey.Description.GetKey(), description);
        }

        public BranchEvent SetSearchQuery(string searchQuery) {
            return AddStandardProperty(BranchJsonKey.SearchQuery.GetKey(), searchQuery);
        }

        private BranchEvent AddStandardProperty(string propertyName, string propertyValue) {
            if (propertyValue != null) {
                try {
                    this.standartProperties.Add(propertyName, JsonValue.CreateStringValue(propertyValue));
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            } else {
                this.standartProperties.Remove(propertyName);
            }
            return this;
        }

        public BranchEvent AddCustomDataProperty(string propertyName, string propertyValue) {
            try {
                this.customProperties.Add(propertyName, JsonValue.CreateStringValue(propertyValue));
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
            string requestPath = isStandartEvent ? RequestPath.TrackStandardEvent.GetPath() : RequestPath.TrackCustomEvent.GetPath();
            BranchServerRequest request = new BranchServerLogEvent(requestPath, eventName, isStandartEvent, buoList, standartProperties, customProperties);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }
    }
}
