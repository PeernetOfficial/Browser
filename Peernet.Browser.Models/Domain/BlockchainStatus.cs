namespace Peernet.Browser.Models.Domain
{
    public enum BlockchainStatus
    {
        BlockchainStatusOK = 0, // No problems in the blockchain detected.
        BlockchainStatusBlockNotFound = 1, // Missing block in the blockchain.
        BlockchainStatusCorruptBlock = 2, // Error block encoding
        BlockchainStatusCorruptBlockRecord = 3, // Error block record encoding
        BlockchainStatusDataNotFound = 4, // Requested data not available in the blockchain
    }
}