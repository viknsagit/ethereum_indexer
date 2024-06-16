# Ethereum Blockchain Indexer

## Overview

Ethereum Blockchain Indexer is a .NET 8-based application designed to monitor and index activities on the Ethereum blockchain. It tracks new blocks, transactions, and contract interactions, storing relevant data in a database for analysis and querying.

### Features

- **Block Tracking**: Monitors new blocks as they are mined on the Ethereum network.
- **Transaction Indexing**: Records details of transactions, including sender, receiver, and status.
- **Contract Monitoring**: Detects and analyzes interactions with Ethereum smart contracts.
- **ERC20 Token Detection**: Identifies ERC20 tokens and tracks their activities on the blockchain.

### Future Enhancements

- **Support for Additional Contract Types**: Expand capabilities to include monitoring and indexing of various smart contract types beyond ERC20 tokens.
- **Performance Optimization**: Integrate Redis for caching to enhance data retrieval speed and reduce database load.
- **Contract Validation**: Implement mechanisms to validate the integrity and correctness of smart contracts during interactions.
- **Database Flexibility**: Add support for multiple databases such as MySQL, PostgreSQL, and SQLite for improved deployment flexibility.

## Getting Started

### Prerequisites

- .NET 8 SDK or higher
- Ethereum node or access to an Infura endpoint for Ethereum RPC communication
- Database server (PostgreSQL)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/viknsagit/ethereum_indexer.git
   cd ethereum_indexer
   ```

2. **Configure appsettings.json:**

   Create a file named `appsettings.json` in the root directory of the project and configure it with your database connection string, Ethereum RPC URL, and WebSocket URL:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "YOUR_DATABASE_CONNECTION_STRING"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "port": 5006,
     "reindex": false,
     "rpc": "YOUR_ETHEREUM_RPC_URL",
     "ws": "YOUR_ETHEREUM_WS_URL",
     "chain": 0000
   }
   ```

   Replace `YOUR_DATABASE_CONNECTION_STRING` with your actual database connection string. Set `YOUR_ETHEREUM_RPC_URL` and `YOUR_ETHEREUM_WS_URL` to your Ethereum RPC and WebSocket URLs respectively.

3. **Install dependencies:**

   ```bash
   dotnet restore
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

## Usage

Once the application is running, it will start monitoring new blocks and transactions on the Ethereum blockchain. Data about transactions, wallets, smart contracts, and ERC20 tokens will be indexed and stored in the configured database for further analysis.

## Contribution

Contributions to the project are welcome! If you encounter issues or have suggestions for improvements, please open an issue or submit a pull request on the GitHub repository: [ethereum_indexer](https://github.com/viknsagit/ethereum_indexer).

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

This README provides a comprehensive overview of your Ethereum Blockchain Indexer project, including setup instructions, features, future enhancements, and guidelines for contribution. Adjustments were made to incorporate future plans for database flexibility as discussed.