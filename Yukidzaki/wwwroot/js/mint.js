const mintButton = document.getElementById('mintButton');
//let responseDataFromAJAXRequest;
mintButton.addEventListener('click', async () => {
    const { ethers } = require('ethers');

    // Check if MetaMask is installed
    if (window.ethereum) {
        const provider = new ethers.providers.Web3Provider(window.ethereum);

        // Request user's permission to access their account
        await window.ethereum.request({ method: 'eth_requestAccounts' });
    } else {
        // Handle case when MetaMask is not installed
        console.error('Please install MetaMask to use this application.');
    }
    const contract = new ethers.Contract(contractAddress, contractAbi, signer);

    // Call the publicMint function by creating a transaction object
    const numberOfTokens = 5; // Set the number of tokens to mint
    const transaction = await contract.publicMint(numberOfTokens, {
        value: publicMintPrice.mul(numberOfTokens),
    });
});