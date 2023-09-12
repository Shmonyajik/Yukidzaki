window.userAddress = null;
window.onload = async () => {
    // Init Web3 connected to ETH network
    console.log("onload!")
    if (window.ethereum) {
        debugger
        window.web3 = new Web3(window.ethereum);
        chainId = await ethereum.request({ method: 'eth_chainId' });
        if (chainId === '0x1') {
            window.userAddress = window.localStorage.getItem("userAddress");
            //showAddress();
        }
        else {
            console.log("ElseOnload")
        }

    } else {
        alert("No ETH brower extension detected.");
    }
}
    // Load in Localstore key
   

// Use this function to turn a 42 character ETH address
// into an address like 0x345...12345
function truncateAddress(address) {
    if (!address) {
        return "";
    }
    return `${address.substr(0, 5)}...${address.substr(
        address.length - 5,
        address.length
    )}`;
}



// Login with Web3 via Metamasks window.ethereum library
async function loginWithEth() {
    if (window.ethereum) {
        window.web3 = new Web3(window.ethereum);
        
        // Load in Localstore key
        checkChainId()
    
            try {
                // We use this since ethereum.enable() is deprecated. This method is not
                // available in Web3JS - so we call it directly from metamasks' library
                debugger
                const selectedAccount = await window.ethereum
                    .request({
                        method: "eth_requestAccounts",
                    })
                    .then((accounts) => accounts[0])
                    .catch(() => {
                        throw Error("No account selected!");
                    });


    /////////////////Проверка подписи на бэке
                const oneTimeCode = await $.ajax({
                    url: '/MetaMaskAuth/GenerateOneTimeCode',
                    type: 'GET',
                    dataType: 'json',
                    success: function (response) {
                        console.log("response:", response)


                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        throw Error(errorThrown);
                    }
                })

                signAndSendSignature()

                async function signAndSendSignature() {
                
                    const signature = await window.web3.eth.personal.sign(oneTimeCode.Value, selectedAccount, ''); // Sign the code with MetaMask
                    console.log("signature: " + signature)
                    const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
                    console.log("antiForgeryToken: " + antiForgeryToken)
                    VerifySignatureRequest = {
                        walletAddress: selectedAccount,
                        oneTimeCode: oneTimeCode.Value,
                        signature: signature
                    }
                    if (antiForgeryToken) {
                        return new Promise((resolve, reject) => {
                            const xhr = new XMLHttpRequest()
                            xhr.open('POST', '/MetaMaskAuth/VerifySignature')
                            xhr.responseType = 'json'
                            xhr.setRequestHeader('Content-Type', 'application/json')
                            xhr.setRequestHeader('X-ANTI-FORGERY-TOKEN', antiForgeryToken)
                            debugger
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
                                debugger
                                window.userAddress = selectedAccount;
                                window.localStorage.setItem("userAddress", selectedAccount);
                                //showAddress();
                                console.log("7" + response);
                            })
                            .catch(error => {
                                // Handle errors
                                console.error("Ошибка проверки подписи" + error);
                            });
                    }
                    else {
                        throw Error('Anti-forgery token not found.');
                    }

                }
        


    /////////////////////////////////////////
            
            } catch (error) {
                alert(error);
            }
    } else {
        alert("No ETH browser extension detected.");
    }
}

async function mint() {
    const CONTRACT_ADDRESS = "0xF416fAa0185070AE542c795f41EC4580Dd35C584";
    const contract = new window.web3.eth.Contract(
        window.ABI,
        CONTRACT_ADDRESS
    );
    const mintPrice = await contract.methods
        .publicMintPrice() 
        .call({ from: window.userAddress }) * 100;
    const hash = contract.methods.safeMint(2).send({ from: window.userAddress, value: mintPrice })
    alert(`Transaction hash: ${hash}`);
}

//ethereum.on('chainChanged', (chainId) => {
//    checkChainId(chainId);
//});

function checkChainId(chainId) {
    // chainId содержит идентификатор текущей сети
    if (chainId === '0x1') {
        // Ethereum Mainnet
        console.log('Подключено к Ethereum Mainnet');

    } else if (chainId === '0x90') {
        // Sepolia
        console.log('Подключено к Sepolia');
        switchToEthereumMainnet()
    } else {
        // Другая сеть
        console.log('Подключено к другой сети');
        switchToEthereumMainnet()
    }
}

 //Функция для изменения сети на Ethereum Mainnet
 function switchToEthereumMainnet() {
    try {
        ethereum.request({
            method: 'wallet_switchEthereumChain',
            params: [{ chainId: '0x1' }], // Идентификатор Ethereum Mainnet
        });
        console.log('Переключено на Ethereum Mainnet');
    } catch (error) {
        console.error('Ошибка при переключении на Ethereum Mainnet:', error);
    }
}
function switchToEthereumMainnet() {
    
    
    debugger
    ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [{ chainId: '0x1' }], // Идентификатор Ethereum Mainnet
    }).catch((err) => {
        debugger
        //console.error('Ошибка при переключении на Arbitrum One:', error);
        if (err.code === 4902) {
            window.ethereum.request({
                method: "wallet_addEthereumChain",
                params: [
                    {
                        chainId: "0x1",
                        rpcUrls: ["https://mainnet.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874", "https://mainnet.infura.io/v3/"],
                        chainName: "Ethereum Mainnet",
                        nativeCurrency: { name: "ETH", decimals: 18, symbol: "ETH" },
                    },
                ],
            });
        }
    });
    
    //console.log('Переключено на Arbitrum One');
    
}

// Функция для изменения сети на Sepolia
// function switchToSepolia() {
//    try {
//         ethereum.request({
//            method: 'wallet_switchEthereumChain',
//            params: [{ chainId: '0x90' }], // Идентификатор Sepolia
//        });
//        console.log('Переключено на Sepolia');
//    } catch (error) {
//        console.error('Ошибка при переключении на Sepolia:', error);
//    }
//}


window.ABI = [
    {
        "inputs": [
            {
                "internalType": "string",
                "name": "customBaseURI",
                "type": "string"
            }
        ],
        "stateMutability": "nonpayable",
        "type": "constructor"
    },
    {
        "inputs": [],
        "name": "ApprovalCallerNotOwnerNorApproved",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "ApprovalQueryForNonexistentToken",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "ApprovalToCurrentOwner",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "ApproveToCaller",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "BalanceQueryForZeroAddress",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "MintToZeroAddress",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "MintZeroQuantity",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "OwnerQueryForNonexistentToken",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "TransferCallerNotOwnerNorApproved",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "TransferFromIncorrectOwner",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "TransferToNonERC721ReceiverImplementer",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "TransferToZeroAddress",
        "type": "error"
    },
    {
        "inputs": [],
        "name": "URIQueryForNonexistentToken",
        "type": "error"
    },
    {
        "anonymous": false,
        "inputs": [
            {
                "indexed": true,
                "internalType": "address",
                "name": "owner",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "address",
                "name": "approved",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "Approval",
        "type": "event"
    },
    {
        "anonymous": false,
        "inputs": [
            {
                "indexed": true,
                "internalType": "address",
                "name": "owner",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "address",
                "name": "operator",
                "type": "address"
            },
            {
                "indexed": false,
                "internalType": "bool",
                "name": "approved",
                "type": "bool"
            }
        ],
        "name": "ApprovalForAll",
        "type": "event"
    },
    {
        "anonymous": false,
        "inputs": [
            {
                "indexed": true,
                "internalType": "address",
                "name": "previousOwner",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "address",
                "name": "newOwner",
                "type": "address"
            }
        ],
        "name": "OwnershipTransferred",
        "type": "event"
    },
    {
        "anonymous": false,
        "inputs": [
            {
                "indexed": true,
                "internalType": "address",
                "name": "from",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "address",
                "name": "to",
                "type": "address"
            },
            {
                "indexed": true,
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "Transfer",
        "type": "event"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "",
                "type": "address"
            }
        ],
        "name": "allowList",
        "outputs": [
            {
                "internalType": "bool",
                "name": "",
                "type": "bool"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "allowListMintOpen",
        "outputs": [
            {
                "internalType": "bool",
                "name": "",
                "type": "bool"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "allowListMintPrice",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "to",
                "type": "address"
            },
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "approve",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "owner",
                "type": "address"
            }
        ],
        "name": "balanceOf",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address[]",
                "name": "addresses",
                "type": "address[]"
            }
        ],
        "name": "delAllowList",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "bool",
                "name": "_publicMintOpen",
                "type": "bool"
            },
            {
                "internalType": "bool",
                "name": "_allowListMintOpen",
                "type": "bool"
            },
            {
                "internalType": "uint256",
                "name": "_maxMintSupply",
                "type": "uint256"
            },
            {
                "internalType": "uint256",
                "name": "_maxMintPerUser",
                "type": "uint256"
            },
            {
                "internalType": "uint256",
                "name": "_allowListMintPrice",
                "type": "uint256"
            },
            {
                "internalType": "uint256",
                "name": "_publicMintPrice",
                "type": "uint256"
            }
        ],
        "name": "editMintWindows",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "getApproved",
        "outputs": [
            {
                "internalType": "address",
                "name": "",
                "type": "address"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "owner",
                "type": "address"
            },
            {
                "internalType": "address",
                "name": "operator",
                "type": "address"
            }
        ],
        "name": "isApprovedForAll",
        "outputs": [
            {
                "internalType": "bool",
                "name": "",
                "type": "bool"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "maxMintPerUser",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "maxMintSupply",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "name",
        "outputs": [
            {
                "internalType": "string",
                "name": "",
                "type": "string"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "owner",
        "outputs": [
            {
                "internalType": "address",
                "name": "",
                "type": "address"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "ownerOf",
        "outputs": [
            {
                "internalType": "address",
                "name": "",
                "type": "address"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "publicMintOpen",
        "outputs": [
            {
                "internalType": "bool",
                "name": "",
                "type": "bool"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "publicMintPrice",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "renounceOwnership",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "quantity",
                "type": "uint256"
            }
        ],
        "name": "safeMint",
        "outputs": [],
        "stateMutability": "payable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "quantity",
                "type": "uint256"
            }
        ],
        "name": "safeMintForWhitelist",
        "outputs": [],
        "stateMutability": "payable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "from",
                "type": "address"
            },
            {
                "internalType": "address",
                "name": "to",
                "type": "address"
            },
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "safeTransferFrom",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "from",
                "type": "address"
            },
            {
                "internalType": "address",
                "name": "to",
                "type": "address"
            },
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            },
            {
                "internalType": "bytes",
                "name": "_data",
                "type": "bytes"
            }
        ],
        "name": "safeTransferFrom",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address[]",
                "name": "addresses",
                "type": "address[]"
            }
        ],
        "name": "setAllowList",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "operator",
                "type": "address"
            },
            {
                "internalType": "bool",
                "name": "approved",
                "type": "bool"
            }
        ],
        "name": "setApprovalForAll",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "string",
                "name": "customBaseURI",
                "type": "string"
            }
        ],
        "name": "setBaseURI",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "bytes4",
                "name": "interfaceId",
                "type": "bytes4"
            }
        ],
        "name": "supportsInterface",
        "outputs": [
            {
                "internalType": "bool",
                "name": "",
                "type": "bool"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "symbol",
        "outputs": [
            {
                "internalType": "string",
                "name": "",
                "type": "string"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "tokenURI",
        "outputs": [
            {
                "internalType": "string",
                "name": "",
                "type": "string"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "totalSupply",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "view",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "from",
                "type": "address"
            },
            {
                "internalType": "address",
                "name": "to",
                "type": "address"
            },
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "transferFrom",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "address",
                "name": "newOwner",
                "type": "address"
            }
        ],
        "name": "transferOwnership",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [],
        "name": "withDraw",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    }
]