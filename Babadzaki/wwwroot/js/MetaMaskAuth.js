window.addEventListener('DOMContentLoaded', () => {
    console.log("DOMContentLoaded");
    if (typeof window.ethereum !== 'undefined') {
        // MetaMask extension is installed
        const ethereum = window.ethereum;
        ethereum.enable(); // Request user permission to access their accounts
        const web3 = new Web3(ethereum);
        signAndSendSignature();
        // Sign the one-time code and send the signature to the server
        async function signAndSendSignature() {
            console.log("Click button!");
            const accounts = await web3.eth.getAccounts();
            const walletAddress = accounts[0];

            const oneTimeCode = $.ajax({
                url: '/MetaMaskAuth/GenerateOneTimeCode',
                type: 'GET',
                success: function (response) {
                    console.log("success" + response);
                    return response;
                }
            });
            console.log("success" + response);
            const signature = await web3.eth.personal.sign(oneTimeCode, walletAddress, ''); // Sign the code with MetaMask

            // Send the signature to the server
            fetch('/YourController/VerifySignature', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    walletAddress: walletAddress,
                    oneTimeCode: oneTimeCode,
                    signature: signature
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

        // Call the signAndSendSignature function when needed (e.g., on a button click)
        // ...
    }
});

//function signAndSendSignatureCall() {
//    signAndSendSignature();
//}