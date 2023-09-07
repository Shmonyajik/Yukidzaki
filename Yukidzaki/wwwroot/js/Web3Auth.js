
window.addEventListener('DOMContentLoaded', () => {
    if (typeof window.ethereum !== 'undefined') {
        console.log("-DOMContentLoaded")
        // MetaMask extension is installed
        const ethereum = window.ethereum;
        ethereum.enable(); // Request user permission to access their accounts
        const web3 = new Web3(ethereum);
        console.log("web3: " + web3)


        try {
            const responseData = await generateCode();
            // Use the response value
            console.log(responseData);

            // Assign the response value to a variable
            const oneTimeCode = JSON.stringify(responseData); 
        } catch (error) {
            // Handle errors
            console.error(error);
        }



        //const oneTimeCode = generateCode().then(responseData => {
        //    return JSON.stringify(responseData);
        //}); // Replace with the actual one-time code
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
            const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
            console.log("antiForgeryToken: " + antiForgeryToken)
            VerifySignatureRequest = {
                walletAddress: walletAddress,
                oneTimeCode: oneTimeCode,
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

        // Call the signAndSendSignature function when needed (e.g., on a button click)
        // ...
    }
});


async function generateCode() {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/MetaMaskAuth/GenerateOneTimeCode',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log("success", response.Value);
                resolve(response.Value);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown); // Reject the promise with the error message
            }
        })
            
    })
}


