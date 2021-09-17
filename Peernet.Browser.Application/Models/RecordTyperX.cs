namespace Peernet.Browser.Application.Models
{
    public enum RecordTyperX
    {
        RecordTypeProfile = 0, // Profile data about the end user
        RecordTypeTagData = 1, // Tag data record to be referenced by one or multiple tags. Only valid in the context of the current block.
        RecordTypeFile = 2, // File
        RecordTypeDelete = 3, // Delete previous record by ID.
        RecordTypeCertificate = 4, // Certificate to certify provided information in the blockchain issued by a trusted 3rd party.
        RecordTypeContentRating = 5, // Content rating (positive).
        RecordTypeContentReport = 6 // Content report (negative).
    }
}