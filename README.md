# Yukidzaki
## Decentralized web application based on ASP.NET Core MVC.
## The current version: 
Implemented features a token gallery with filters and search. The original images are downloaded from the ipfs server https://bafybeiae2a4skkbqam5qbgoprbzemj5kaotl76bb6zdr6i7eqrbt42wfra.ipfs.nftstorage.link/.
Loading information about tokens into the database is implemented using JSON files generated based on the attributes of pictures (tokens). A feedback form and mailing via smtp.gmail.com have also been implemented.
Using the Web3.js library, primary interaction with the smart contract and the Metamask provider was implemented. By connecting your wallet, you can mint one or more tokens. Smart contract on Ethereum network:
https://sepolia.etherscan.io/address/0xf416faa0185070ae542c795f41ec4580dd35c584#code.
To fully interact with the web application, you need to install the MetaMask browser extension and create a wallet: https://metamask.io/download/.
## In the next version:
* A full-fledged user account with collection display and personal content
* Authorization using other providers (Coinbase Wallet, WalletConnect, Phantom)
## Running with Docker
###  To run a web application on your local machine, follow these steps:
* Download the Docker image https://hub.docker.com/r/shmonyajik/yukidzaki_app/tags
  
``` docker pull shmonyajik/yukidzaki_app:dev ```
* Download the docker-compose.yml file from this repository https://github.com/Shmonyajik/Yukidzaki/tree/master/Yukidzaki/LocalLaunch
* Then run in the directory with the file:
  
``` docker-compose up ```

* Go to http://localhost:80
