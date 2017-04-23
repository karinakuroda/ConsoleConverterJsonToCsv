using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleConverter
{
    public class Converter
    {
        public Item ItemDeserialized { get; set; }
        public string LoadJson(string nomeArquivo, string fileNameWithoutExtension, string file)
        {
            var stringProcessed = "";

            byte[] byteArray = Encoding.UTF8.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using (StreamReader r = new StreamReader(stream))
            {
                string json = r.ReadToEnd();
                var resp = JsonConvert.DeserializeObject<Item>(json);
                ItemDeserialized = resp;
                stringProcessed = Process(fileNameWithoutExtension, resp);
            }
            return stringProcessed;
        }
        public string Process(string fileNameWithoutExtension, Item itemDeserialized)
        {
            var stringProcessed = "";
            var index = 0;

            if (itemDeserialized != null && itemDeserialized.transferBatch != null && itemDeserialized.transferBatch.callEventDetails != null)
            {


                foreach (var callEventDetail in itemDeserialized.transferBatch.callEventDetails)
                {

                    index += 1;
                    if (callEventDetail.gprsCall != null)
                    {
                        stringProcessed += String.Format("{0};{1};{2};{3}{4}",
                        fileNameWithoutExtension,
                        "GPRS",
                          callEventDetail.gprsCall.gprsBasicCallInformation.callEventStartTimeStamp.localTimeStamp,
                        callEventDetail.gprsCall.gprsBasicCallInformation.gprsChargeableSubscriber.chargeableSubscriber.simChargeableSubscriber.imsi,
                        Environment.NewLine);
                    }
                    else if (callEventDetail.mobileOriginatedCall != null)
                    {
                        stringProcessed += ProcessMobileOriginatedCall(fileNameWithoutExtension, callEventDetail.mobileOriginatedCall);
                    }
                    else if (callEventDetail.mobileTerminatedCall != null)
                    {
                        stringProcessed += ProcessMobileTerminatedCall(fileNameWithoutExtension, callEventDetail.mobileTerminatedCall);
                    }

                }
            }
            return stringProcessed;
        }
        public string ProcessMobileOriginatedCall(string fileNameWithoutExtension, mobileOriginatedCall mobileOriginatedCall)
        {
            var type = mobileOriginatedCall.basicServiceUsedList[0].basicService.serviceCode.teleServiceCode;
            var typeDescr = "";
            switch (type)
            {
                case 11:
                    typeDescr = "VOZ_MOC";
                    break;
                case 12:
                    typeDescr = "VOZ_MOC_EMER";
                    break;
                case 22:
                    typeDescr = "SMS_MOC";
                    break;
                default:
                    typeDescr = "BEARER_MOC";
                    break;
            }
            var str = String.Format("{0};{1};{2};{3}{4}",
                   fileNameWithoutExtension,
                   typeDescr,
                    mobileOriginatedCall.basicCallInformation.callEventStartTimeStamp.localTimeStamp,
                  mobileOriginatedCall.basicCallInformation.chargeableSubscriber.simChargeableSubscriber.imsi,
                  Environment.NewLine);

            return str;

        }
        public string ProcessMobileTerminatedCall(string fileNameWithoutExtension, mobileTerminatedCall mobileTerminatedCall)
        {
            var type = mobileTerminatedCall.basicServiceUsedList[0].basicService.serviceCode.teleServiceCode;
            var typeDescr = "";
            switch (type)
            {
                case 11:
                    typeDescr = "VOZ_MTC";
                    break;
                case 21:
                    typeDescr = "SMS_MTC";
                    break;
                default:
                    typeDescr = "BEARER_MTC";
                    break;
            }
            var str = String.Format("{0};{1};{2};{3}{4}",
                  fileNameWithoutExtension,
                  typeDescr,
                   mobileTerminatedCall.basicCallInformation.callEventStartTimeStamp.localTimeStamp,
                 mobileTerminatedCall.basicCallInformation.chargeableSubscriber.simChargeableSubscriber.imsi,
                 Environment.NewLine);

            return str;
        }
    }
    #region CLASSES


    //NOME DO ARQUIVO, TIPO, DT CHAMADA, IMSI
    public class Item
    {

        public transferBatch transferBatch;
    }
    public class accountingInfo
    {
        public int tapDecimalPlaces { get; set; }
    }
    public class transferBatch
    {
        public accountingInfo accountingInfo;
        public callEventDetails[] callEventDetails { get; set; }
    }
    public class callEventDetails
    {
        public gprsCall gprsCall { get; set; }
        public mobileOriginatedCall mobileOriginatedCall { get; set; }
        public mobileTerminatedCall mobileTerminatedCall { get; set; }
    }
    public class mobileTerminatedCall
    {
        public basicCallInformation basicCallInformation { get; set; }
        public basicServiceUsedList[] basicServiceUsedList { get; set; }
    }
    public class mobileOriginatedCall
    {
        public basicCallInformation basicCallInformation { get; set; }
        public basicServiceUsedList[] basicServiceUsedList { get; set; }
    }
    public class basicServiceUsedList
    {
        public basicService basicService { get; set; }
    }
    public class basicService
    {
        public serviceCode serviceCode { get; set; }
    }
    public class serviceCode
    {
        public int teleServiceCode { get; set; }
    }
    public class basicCallInformation
    {
        public callEventStartTimeStamp callEventStartTimeStamp { get; set; }
        public chargeableSubscriber chargeableSubscriber { get; set; }
    }

    public class gprsCall
    {
        public gprsBasicCallInformation gprsBasicCallInformation { get; set; }
    }
    public class gprsBasicCallInformation
    {
        public gprsChargeableSubscriber gprsChargeableSubscriber { get; set; }
        public callEventStartTimeStamp callEventStartTimeStamp { get; set; }
    }
    public class callEventStartTimeStamp
    {
        public string localTimeStamp { get; set; }
    }
    public class gprsChargeableSubscriber
    {
        public string pdpAddress { get; set; }
        public chargeableSubscriber chargeableSubscriber { get; set; }
    }
    public class chargeableSubscriber
    {
        public simChargeableSubscriber simChargeableSubscriber { get; set; }
    }
    public class simChargeableSubscriber
    {
        public string imsi { get; set; }
    }
    #endregion
}
