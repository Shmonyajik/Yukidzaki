
window.addEventListener('DOMContentLoaded', () => {
    if (typeof window.ethereum !== 'undefined') {
        console.log("-DOMContentLoaded")
        // MetaMask extension is installed
        const ethereum = window.ethereum;
        ethereum.enable(); // Request user permission to access their accounts
        const web3 = new Web3(ethereum);
        console.log("web3: " + web3)
        generateCode();
        const oneTimeCode = generateCode(); // Replace with the actual one-time code
        console.log(oneTimeCode);
        signAndSendSignature();
        // Sign the one-time code and send the signature to the server
        async function signAndSendSignature() {
            console.log("signAndSendSignature");
            const accounts = await web3.eth.getAccounts();
            console.log("accounts: " + accounts)
            const walletAddress = accounts[0];
            console.log("Address: "+walletAddress)
            const signature = await web3.eth.personal.sign(oneTimeCode, walletAddress, ''); // Sign the code with MetaMask
            console.log("signature: " + signature)
            VerifySignatureRequest = {
                "walletAddress": walletAddress,
                "oneTimeCode": oneTimeCode,
                "signature": signature
                }
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
                    console.log("7" + response);
                })
                .catch(error => {
                    // Handle errors
                    console.error("7" + error);
                });
        }

        // Call the signAndSendSignature function when needed (e.g., on a button click)
        // ...
    }
});

async function generateCode() {
    console.log("Click button!");

    return $.ajax({
        url: '/MetaMaskAuth/GenerateOneTimeCode',
        type: 'GET',
        success: function (response) {
            console.log(response.Value);
            return response.Value;
        }
    });
}
