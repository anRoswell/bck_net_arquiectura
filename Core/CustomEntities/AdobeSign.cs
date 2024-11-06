using System;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class BaseUriClass
    {
        public string ApiAccessPoint { get; set; }
        public string WebAccessPoint { get; set; }
    }

    public class TransientDocument
    {
        public string TransientDocumentId { get; set; }
    }

    #region Agreements
    public class Agreements
    {
        public List<Fileinfo> fileInfos { get; set; }
        public string name { get; set; }
        public List<CCInfo> ccs { get; set; }
        public List<Participantsetsinfo_Agree> participantSetsInfo { get; set; }
        public string signatureType { get; set; }
        public string state { get; set; }
    }

    public class Fileinfo
    {
        public string transientDocumentId { get; set; }
    }

    public class Participantsetsinfo_Agree
    {
        public List<Memberinfo_Agree> memberInfos { get; set; }
        public int order { get; set; }
        public string role { get; set; }
    }

    public class CCInfo
    {
        public string email { get; set; }
    }

    public class Memberinfo_Agree
    {
        public string email { get; set; }
    }

    public class AgreementsResponse
    {
        public string Id { get; set; }
    }
    #endregion

    #region ValidateDocument
    public class ValidateDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GroupId { get; set; }
        public string Type { get; set; }
        public List<Participantsetsinfo_ValDoc> ParticipantSetsInfo { get; set; }
        public string SenderEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEventDate { get; set; }
        public string SignatureType { get; set; }
        public string Locale { get; set; }
        public string Status { get; set; }
        public bool DocumentVisibilityEnabled { get; set; }
        public bool HasFormFieldData { get; set; }
        public bool HasSignerIdentityReport { get; set; }
        public Agreementsettingsinfo AgreementSettingsInfo { get; set; }
        public bool DocumentRetentionApplied { get; set; }
    }

    public class Agreementsettingsinfo
    {
        public bool CanEditFiles { get; set; }
        public bool CanEditElectronicSeals { get; set; }
        public bool CanEditAgreementSettings { get; set; }
    }

    public class Participantsetsinfo_ValDoc
    {
        public string Id { get; set; }
        public List<Memberinfo_ValDoc> MemberInfos { get; set; }
        public string Role { get; set; }
        public int Order { get; set; }
    }

    public class Memberinfo_ValDoc
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Securityoption SecurityOption { get; set; }
    }

    public class Securityoption
    {
        public string AuthenticationMethod { get; set; }
    }
    #endregion

    public class adobeErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
