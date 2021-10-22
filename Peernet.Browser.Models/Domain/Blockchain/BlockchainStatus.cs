namespace Peernet.Browser.Models.Domain.Blockchain
{
    public enum BlockchainStatus
    {
        StatusOK = 0, // No problems in the blockchain detected.
        StatusBlockNotFound = 1, // Missing block in the blockchain.
        StatusCorruptBlock = 2, // Error block encoding
        StatusCorruptBlockRecord = 3, // Error block record encoding
        StatusDataNotFound = 4, // Requested data not available in the blockchain
        StatusNotInWarehouse = 5 // File to be added to blockchain does not exist in the Warehouse
    }
}