
window.addEventListener('DOMContentLoaded', () => {
    console.log("DOMContentLoaded");
    if (typeof window.ethereum !== 'undefined') {
        // MetaMask extension is installed
        const ethereum = window.ethereum;
        ethereum.enable(); // Request user permission to access their accounts
        const web3 = new Web3(ethereum);
        const accounts = web3.eth.getAccounts();
        const walletAddress = accounts[0];
        oneTimeCode = signAndSendSignature();
        // Sign the one-time code and send the signature to the server
        
            
        const signature = web3.eth.personal.sign(oneTimeCode, walletAddress, ''); // Sign the code with MetaMask

        sendSignatureToServer(walletAddress, oneTimeCode, signature);

        // Call the signAndSendSignature function when needed (e.g., on a button click)
        // ...
    }
});

//function signAndSendSignatureCall() {
//    signAndSendSignature();
//}
async function signAndSendSignature() {
    console.log("Click button!");

    return $.ajax({
        url: '/MetaMaskAuth/GenerateOneTimeCode',
        type: 'GET',
        success: function (response) {
            return response;
        }
    });
}

async function sendSignatureToServer(walletAddress, oneTimeCode, signature) {
    
    // Send the signature to the server
    fetch('/MetaMaskAuth/VerifySignature', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            WalletAddress: walletAddress,
            OneTimeCode: oneTimeCode,
            Signature: signature
        })
    })
        .then(response => {
            // Handle the server response
            
            console.log(response);
        })
        .catch(error => {
            // Handle errors
            console.error(error);
        });
}