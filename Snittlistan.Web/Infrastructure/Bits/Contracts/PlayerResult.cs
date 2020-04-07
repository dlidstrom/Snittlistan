namespace Snittlistan.Web.Infrastructure.Bits.Contracts
{
    using Newtonsoft.Json;

    public class PlayerResult
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("data")]
        public PlayerItem[] Data { get; set; }
    }

    public class PlayerItem
    {
        [JsonProperty("licNbr")]
        public string LicNbr { get; set; }

        [JsonProperty("pnr")]
        public string Pnr { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("surName")]
        public string SurName { get; set; }

        [JsonProperty("adress")]
        public string Adress { get; set; }

        [JsonProperty("coAdress")]
        public string CoAdress { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phoneHome")]
        public string PhoneHome { get; set; }

        [JsonProperty("phoneWork")]
        public string PhoneWork { get; set; }

        [JsonProperty("phoneMobile")]
        public string PhoneMobile { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("countyId")]
        public long CountyId { get; set; }

        [JsonProperty("clubId")]
        public long ClubId { get; set; }

        [JsonProperty("licType")]
        public long LicType { get; set; }

        [JsonProperty("oldHcp")]
        public long OldHcp { get; set; }

        [JsonProperty("hcp")]
        public long Hcp { get; set; }

        [JsonProperty("sex")]
        public Sex Sex { get; set; }

        [JsonProperty("hcpEstimateDate")]
        public string HcpEstimateDate { get; set; }

        [JsonProperty("hcpEstimate")]
        public long HcpEstimate { get; set; }

        [JsonProperty("licenceAgreementSeason")]
        public object LicenceAgreementSeason { get; set; }

        [JsonProperty("licenceAgreementLicNbr")]
        public object LicenceAgreementLicNbr { get; set; }

        [JsonProperty("clubName")]
        public string ClubName { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("licTypeId")]
        public long LicTypeId { get; set; }

        [JsonProperty("licTypeName")]
        public string LicTypeName { get; set; }

        [JsonProperty("licenceHcpDate")]
        public string LicenceHcpDate { get; set; }

        [JsonProperty("licenceSkillLevel")]
        public double LicenceSkillLevel { get; set; }

        [JsonProperty("licenceAverage")]
        public double LicenceAverage { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }
    }

    public enum Sex
    {
        M,
        K
    }
}
