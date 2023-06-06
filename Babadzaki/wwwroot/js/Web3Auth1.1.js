const button = document.getElementById('connectWalletButton');
//let responseDataFromAJAXRequest;
button.addEventListener('click', async () => {
    
    console.log('Button clicked');

    if (typeof window.ethereum !== 'undefined') {
        console.log("-DOMContentLoaded")
        const ethereum = window.ethereum
        ethereum.enable(); // Request user permission to access their accounts
        const web3 = new Web3(ethereum)
        console.log("web3: " + web3)
        
        
        const oneTimeCode = await $.ajax({
            url: '/MetaMaskAuth/GenerateOneTimeCode',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log("response:", response)
                

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown) // Reject the promise with the error message
            }
        })

        /*console.log("oneTimeCode: " + oneTimeCode)*/
        console.log("oneTimeCode: " + oneTimeCode.Value)
        
        signAndSendSignature()
        async function signAndSendSignature() {
            console.log("signAndSendSignature");
            const accounts = await web3.eth.getAccounts();
            console.log("accounts: " + accounts)
            const walletAddress = accounts[0];
            console.log("Address: " + walletAddress)
            const signature = await web3.eth.personal.sign(oneTimeCode, walletAddress, ''); // Sign the code with MetaMask
            console.log("signature: " + signature)
            const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
            console.log("antiForgeryToken: " + antiForgeryToken)
            VerifySignatureRequest = {
                walletAddress: walletAddress,
                oneTimeCode: oneTimeCode.Value,
                signature: signature
            }
            if (antiForgeryToken) {
                return new Promise((resolve, reject) => {
                    const xhr = new XMLHttpRequest()
                    xhr.open('POST', '/MetaMaskAuth/VerifySignature')
                    xhr.responseType = 'json'
                    xhr.setRequestHeader('Content-Type', 'application/json')
                    xhr.setRequestHeader('RequestVerificationToken', 'antiForgeryToken')

                    xhr.onload = () => {
                        if (xhr.status >= 400) {
                            reject(xhr.response)
                        }
                        else
                            resolve(xhr.response)
                    }
                    xhr.onerror = () => {
                        reject(xhr.response)
                    }
                    xhr.send(JSON.stringify(VerifySignatureRequest))

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
            else {
                console.error('Anti-forgery token not found.');
            }

        }
    }

    
});