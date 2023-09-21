window.userAddress = null;
let elementToDelete = document.getElementById("wrapper");
var gasPriceInInput = null;
var mintPriceInInput = null;
var publicMintOpen = false;
    // Load in Localstore key
window.onload = async () => {
    // Init Web3 connected to ETH network
    console.log("onload");
    if (window.ethereum) {
        window.web3 = new Web3(window.ethereum);
        const accounts = await window.ethereum.request({ method: 'eth_accounts' });

        if (Array.isArray(accounts) && accounts.length > 0) {
            window.ethereum.request({ method: 'eth_chainId' })
                .then(chainIdHex => {
                    // Convert the hexadecimal chainId to a decimal number
                    /*const chainId = parseInt(chainIdHex, 16).toString();*/
                    console.log('Chain ID:', chainIdHex);
                    checkChainId(chainIdHex);
                    
                })
                .catch(error => {
                    console.error('Error getting chain ID from MetaMask:', error);
                });
            
            
            // Load in Localstore key
            window.userAddress = window.localStorage.getItem("userAddress");
            changeBtn(window.userAddress);
            //showAddress();

            // Вы можете продолжить взаимодействие с аккаунтом, например, отправлять транзакции или получать баланс
        }
       
    } else {
        alert("No ETH brower extension detected.");
    }

    
};   

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
        
        
    
            try {
                // We use this since ethereum.enable() is deprecated. This method is not
                // available in Web3JS - so we call it directly from metamasks' library
                
                const selectedAccount = await window.ethereum
                    .request({
                        method: "eth_requestAccounts",
                    })
                    .then((accounts) => accounts[0])
                    .catch(() => {
                        throw Error("No account selected!");
                    });
                window.ethereum.request({ method: 'eth_chainId' })
                    .then(chainIdHex => {
                        // Convert the hexadecimal chainId to a decimal number
                        /*const chainId = parseInt(chainIdHex, 16).toString();*/
                        console.log('Chain ID:', chainIdHex);
                        checkChainId(chainIdHex);

                    })
                    .catch(error => {
                        console.error('Error getting chain ID from MetaMask:', error);
                    });
                window.userAddress = selectedAccount;
                window.localStorage.setItem("userAddress", selectedAccount);


                changeBtn(window.userAddress);

    /////////////////Проверка подписи на бэке
                
        


    /////////////////////////////////////////
            
            } catch (error) {
                alert(error);
            }
    } else {
        alert("No ETH browser extension detected.");
    }
}
async function changeBtn(userAddress) {

    console.log('CHANGE BUTTON!' + userAddress);

    var button = document.getElementById("connectwallet-mint-btn");
    var requestData = {
        userAddress: userAddress
    };
    $.ajax({
        type: 'POST',
        url: '/MetaMaskAuth/GetButton/',
        contentType: 'application/json',
        data: JSON.stringify(requestData),
        success: function (html) {
            console.log("success")
            button.innerHTML = html;
            
            /*modal.modal('show')*/
        },
        failure: function () {
            console.log("failure");
            /* modal.modal('hide')*/
        },
        error: function (response) {
            console.log("error")
            alert(response.responseText)
        },
        complete: function () {
            if (userAddress) {
                document.getElementById("ConnectWalletBtn").textContent = userAddress;
            }
            
        }
    });
        
}
function checkContractInfo() {
    const CONTRACT_ADDRESS = "0xd075875c6C3718c40e0F0eBbBeA46a6e09ec14D1";
    const contract = new window.web3.eth.Contract(
        window.ABI,
        CONTRACT_ADDRESS
    );
    contract.methods.publicMintPrice().call({ from: window.userAddress })
        .then(function (result) {
            console.log('Результат вызова функции publicMintPrice:', result);
            mintPriceInInput = result;
        })
        .catch(function (error) {
            console.error('Ошибка вызова функции publicMintPrice:', error);
        });        
    contract.methods.publicMintOpen().call()
        .then(function (result) {
            console.log('Результат вызова функции publicMintOpen:', result);
            publicMintOpen = result;
        })
        .catch(function (error) {
            console.error('Ошибка вызова функции publicMintOpen:', error);
        });
    

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


////////////////////////////////////////
function handleAccountChange(accounts) {
    console.log("account changed!")
    if (accounts.length > 0) {
        const selectedAccount = accounts[0];
        console.log('Selected account:', selectedAccount);
        changeBtn(selectedAccount)
        // You can perform actions or updates here when the account changes
    } else {
        console.log('No account is currently connected');
        window.userAddress = null;
        unlockUserScreen()
        changeBtn()
        // Handle the case where no account is connected (user locked MetaMask)
    }
}
ethereum.on('chainChanged', (chainId) => {
    console.log("ChainChanged!")
    checkChainId(chainId);
});
// Listen for account changes
window.ethereum.on('accountsChanged', handleAccountChange);
//window.addEventListener('ethereum#initialized', checkWalletAvailability);
///////////////////////////////////////
function checkChainId(chainId) {
    //window.web3 = new Web3(window.ethereum);
    //const accounts = window.ethereum.request({ method: 'eth_accounts' });
    //console.log(accounts[0])
    if (window.userAddress) {
        // chainId содержит идентификатор текущей сети
        if (chainId === '0x1') {

            unlockUserScreen();

            // Ethereum Mainnet


            console.log('Подключено к Ethereum Mainnet');

        } else if (chainId === '0x90') {
            // Sepolia
            //TODO: блокируем экран
            console.log('Подключено к Sepolia');
            switchToEthereumMainnet()
        } else {
            // Другая сеть
            //TODO: блокируем экран
            console.log('Подключено к другой сети');
            switchToEthereumMainnet()
        }
    }
}

 //Функция для изменения сети на Ethereum Mainnet
function switchToEthereumMainnet() {
    blockUserScreen();
    ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [{ chainId: '0x1' }], // Идентификатор Ethereum Mainnet
    })
        .catch((err) => {

            if (err.code === 4001) {
                console.log("Пользователь отказался менять сеть!")
                //blockUserScreen();

            }
            else {
                //blockUserScreen();
                console.log(err);
            }
        });



}
//function checkWalletAvailability() {
//    if (typeof window.ethereum === 'undefined') {
//        console.log('Wallet is disabled or not available.');
//        //changeBtn();
//        // Handle the case where the wallet is disabled or not available
//        //logout();
//    } else {
//        console.log('Wallet is enabled and available.');
//        // Handle the case where the wallet is enabled and available
//    }
//}
function showAddress() {
    if (!window.userAddress) {
        document.getElementById("userAddress").innerText = "";
        document.getElementById("logoutButton").classList.add("hidden");
        return false;
    }
    else {
        //TODO: выводить снова Connect Wallet
    }

}

function logout() {
        window.userAddress = null;
        window.localStorage.removeItem("userAddress");
        showAddress();
}

function blockUserScreen() {
    elementToDelete = document.getElementById("wrapper");
    if (elementToDelete) {
        elementToDelete.innerHTML = "";
    }
    
}
function unlockUserScreen() {
    if (elementToDelete.innerHTML === "") {
        console.log("innerHTML is null")
        location.reload(true);
    }
    
}

function VerifyAccount(callback) {
    $.ajax({
        url: '/MetaMaskAuth/GenerateOneTimeCode',
        type: 'GET',
        dataType: 'json',
        success: function (oneTimeCode) {
            signAndSendSignature(oneTimeCode.Value, callback);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            callback(false, errorThrown);
        }
    });
}

function signAndSendSignature(oneTimeCodeValue, callback) {
    selectedAccount = window.userAddress;
    window.web3.eth.personal.sign(oneTimeCodeValue, selectedAccount, '')
        .then(function (signature) {
            const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

            if (antiForgeryToken) {
                const VerifySignatureRequest = {
                    walletAddress: selectedAccount,
                    oneTimeCode: oneTimeCodeValue,
                    signature: signature
                };

                const xhr = new XMLHttpRequest();
                xhr.open('POST', '/MetaMaskAuth/VerifySignature');
                xhr.responseType = 'json';
                xhr.setRequestHeader('Content-Type', 'application/json');
                xhr.setRequestHeader('X-ANTI-FORGERY-TOKEN', antiForgeryToken);

                xhr.onload = function () {
                    
                    if (xhr.response.Value.StatusCode >= 400) {
                        callback(false, xhr.response.Value.Data);
                    } else {
                        callback(true, xhr.response.Value.Description);
                    }
                };

                xhr.onerror = function () {
                    var response = JSON.parse(xhr.responseText);
                    callback(false, xhr.response.Value.Description);
                };

                xhr.send(JSON.stringify(VerifySignatureRequest));
            } else {
                callback(false, 'Anti-forgery token not found.');
            }
        })
        .catch(function (error) {
            callback(false, error);
        });
}
function OpenModalMint(parameters) {
    console.log("click!!!!")
    GetGusPrice();
     VerifyAccount(function (result, error) {
         console.log(result);
         if (result === true) {
            console.log("Аккаунт верифицирован успешно");
            checkContractInfo();
            if (!publicMintOpen) {
                
                const modal = $('#popupID')

                $.ajax({
                    url: parameters.url,
                    type: 'GET',
                    success: function (response) {
                       
                       console.log("success")
                       modal.find(".modalbody").html(response);
                       modal.modal('show')
                          
                    },
                    failure: function () {
                        console.log("failure");
                        modal.modal('hide')
                    },
                    error: function ( response) {
                        console.log("error")
                        alert(response.responseText)
                       
                    },
                    complete: function () {
                        

                       console.log(gasPriceInInput);
                       document.getElementById("GasPrice").textContent = `Текущая стоимость газа: $${gasPriceInInput}`;
                       document.getElementById("MintPrice").textContent = `Текущая стоимость минта: $${mintPriceInInput}`;
                      
                    }


                });
            }
            else {
                alert("Публичный минт закрыт")
            }
         } else {
             console.error("Верификация аккаунта не пройдена");
        }
    });
    
   
   
}

function GetGusPrice() {
    if (typeof window.ethereum !== 'undefined') {
        const web3 = new Web3(window.ethereum);

        // Запрашиваем разрешение на доступ к аккаунту MetaMask
        window.ethereum.request({ method: 'eth_requestAccounts' })
            .then(() => {
                // Получение актуальной стоимости газа
                web3.eth.getGasPrice()
                    .then(async (gasPrice) => {
                        console.log(`Текущая стоимость газа в Wei: ${gasPrice}`);

                        // Преобразование стоимости газа в Gwei
                        const gasPriceInGwei = web3.utils.fromWei(gasPrice, 'gwei');
                        console.log(`Текущая стоимость газа в Gwei: ${gasPriceInGwei}`);

                        // Получение текущего курса Ethereum к доллару
                        const apiKey = 'YOUR_API_KEY'; // Замените на свой API ключ для доступа к данным курса Ethereum к доллару
                        const apiUrl = `https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd`;

                        try {
                            const response = await fetch(apiUrl);
                            const data = await response.json();
                            const ethToUsdRate = data.ethereum.usd;

                            // Перевод стоимости газа из Gwei в Ether
                            const gasPriceInEther = web3.utils.fromWei(gasPrice, 'ether');
                            
                            // Перевод стоимости газа из Ether в доллары
                            const gasPriceInUsd = gasPriceInEther * ethToUsdRate;
                            gasPriceInInput = gasPriceInUsd;
                            
                            console.log(`Текущая стоимость газа в долларах: $${gasPriceInInput}`);
                            
                        } catch (error) {
                            console.error('Ошибка при получении курса Ethereum к доллару:', error);
                        }
                    })
                    .catch((error) => {
                        console.error('Ошибка при получении стоимости газа:', error);
                    });
            })
            .catch((error) => {
                console.error('Ошибка при запросе разрешения на доступ к аккаунту MetaMask:', error);
            });
    } else {
        console.error('MetaMask не доступен. Убедитесь, что он установлен и активирован в вашем браузере.');
    }
    
    
}

function Mint() {
    $("#tokenForm").submit(function (event) {
        event.preventDefault(); // Предотвращаем отправку формы по умолчанию

        // Получаем значение поля "Количество токенов"
        var tokenCount = $("#tokenCount").val();

        // Проверяем, что значение является положительным числом
        if (isNaN(tokenCount) || tokenCount <= 0) {
            $("#tokenCountError").text("Введите положительное число.");
        } else {
            $("#tokenCountError").text(""); // Очищаем сообщение об ошибке

            // Получаем значение AntiForgeryToken из формы
            var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

            // Отправляем данные на сервер с использованием AJAX и AntiForgeryToken
            $.ajax({
                url: "/your-server-endpoint-url", // Замените на URL вашего сервера
                method: "POST",
                data: { tokenCount: tokenCount },
                headers: { 'RequestVerificationToken': antiForgeryToken }, // Передаем AntiForgeryToken в заголовке
                success: function (response) {
                    // Обработка успешного ответа от сервера
                    console.log("Успешно отправлено на сервер: " + response);
                },
                error: function (error) {
                    // Обработка ошибки
                    console.error("Ошибка при отправке на сервер: " + error);
                }
            });
        }
    });
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


window.ABI = [{ "inputs": [{ "internalType": "string", "name": "customBaseURI", "type": "string" }], "stateMutability": "nonpayable", "type": "constructor" }, { "inputs": [], "name": "ApprovalCallerNotOwnerNorApproved", "type": "error" }, { "inputs": [], "name": "ApprovalQueryForNonexistentToken", "type": "error" }, { "inputs": [], "name": "ApprovalToCurrentOwner", "type": "error" }, { "inputs": [], "name": "ApproveToCaller", "type": "error" }, { "inputs": [], "name": "BalanceQueryForZeroAddress", "type": "error" }, { "inputs": [], "name": "MintToZeroAddress", "type": "error" }, { "inputs": [], "name": "MintZeroQuantity", "type": "error" }, { "inputs": [], "name": "OwnerQueryForNonexistentToken", "type": "error" }, { "inputs": [], "name": "TransferCallerNotOwnerNorApproved", "type": "error" }, { "inputs": [], "name": "TransferFromIncorrectOwner", "type": "error" }, { "inputs": [], "name": "TransferToNonERC721ReceiverImplementer", "type": "error" }, { "inputs": [], "name": "TransferToZeroAddress", "type": "error" }, { "inputs": [], "name": "URIQueryForNonexistentToken", "type": "error" }, { "anonymous": false, "inputs": [{ "indexed": true, "internalType": "address", "name": "owner", "type": "address" }, { "indexed": true, "internalType": "address", "name": "approved", "type": "address" }, { "indexed": true, "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "Approval", "type": "event" }, { "anonymous": false, "inputs": [{ "indexed": true, "internalType": "address", "name": "owner", "type": "address" }, { "indexed": true, "internalType": "address", "name": "operator", "type": "address" }, { "indexed": false, "internalType": "bool", "name": "approved", "type": "bool" }], "name": "ApprovalForAll", "type": "event" }, { "anonymous": false, "inputs": [{ "indexed": true, "internalType": "address", "name": "previousOwner", "type": "address" }, { "indexed": true, "internalType": "address", "name": "newOwner", "type": "address" }], "name": "OwnershipTransferred", "type": "event" }, { "anonymous": false, "inputs": [{ "indexed": true, "internalType": "address", "name": "from", "type": "address" }, { "indexed": true, "internalType": "address", "name": "to", "type": "address" }, { "indexed": true, "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "Transfer", "type": "event" }, { "inputs": [{ "internalType": "address", "name": "", "type": "address" }], "name": "allowList", "outputs": [{ "internalType": "bool", "name": "", "type": "bool" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "allowListMintOpen", "outputs": [{ "internalType": "bool", "name": "", "type": "bool" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "allowListMintPrice", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "to", "type": "address" }, { "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "approve", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "owner", "type": "address" }], "name": "balanceOf", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "address[]", "name": "addresses", "type": "address[]" }], "name": "delAllowList", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "bool", "name": "_publicMintOpen", "type": "bool" }, { "internalType": "bool", "name": "_allowListMintOpen", "type": "bool" }, { "internalType": "uint256", "name": "_maxMintSupply", "type": "uint256" }, { "internalType": "uint256", "name": "_maxMintPerUser", "type": "uint256" }, { "internalType": "uint256", "name": "_allowListMintPrice", "type": "uint256" }, { "internalType": "uint256", "name": "_publicMintPrice", "type": "uint256" }], "name": "editMintWindows", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "getApproved", "outputs": [{ "internalType": "address", "name": "", "type": "address" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "owner", "type": "address" }, { "internalType": "address", "name": "operator", "type": "address" }], "name": "isApprovedForAll", "outputs": [{ "internalType": "bool", "name": "", "type": "bool" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "maxMintPerUser", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "maxMintSupply", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "name", "outputs": [{ "internalType": "string", "name": "", "type": "string" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "owner", "outputs": [{ "internalType": "address", "name": "", "type": "address" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "ownerOf", "outputs": [{ "internalType": "address", "name": "", "type": "address" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "publicMintOpen", "outputs": [{ "internalType": "bool", "name": "", "type": "bool" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "publicMintPrice", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "renounceOwnership", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "uint256", "name": "quantity", "type": "uint256" }], "name": "safeMint", "outputs": [], "stateMutability": "payable", "type": "function" }, { "inputs": [{ "internalType": "uint256", "name": "quantity", "type": "uint256" }], "name": "safeMintForWhitelist", "outputs": [], "stateMutability": "payable", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "from", "type": "address" }, { "internalType": "address", "name": "to", "type": "address" }, { "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "safeTransferFrom", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "from", "type": "address" }, { "internalType": "address", "name": "to", "type": "address" }, { "internalType": "uint256", "name": "tokenId", "type": "uint256" }, { "internalType": "bytes", "name": "_data", "type": "bytes" }], "name": "safeTransferFrom", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "address[]", "name": "addresses", "type": "address[]" }], "name": "setAllowList", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "operator", "type": "address" }, { "internalType": "bool", "name": "approved", "type": "bool" }], "name": "setApprovalForAll", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "string", "name": "customBaseURI", "type": "string" }], "name": "setBaseURI", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "bytes4", "name": "interfaceId", "type": "bytes4" }], "name": "supportsInterface", "outputs": [{ "internalType": "bool", "name": "", "type": "bool" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "symbol", "outputs": [{ "internalType": "string", "name": "", "type": "string" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "tokenURI", "outputs": [{ "internalType": "string", "name": "", "type": "string" }], "stateMutability": "view", "type": "function" }, { "inputs": [], "name": "totalSupply", "outputs": [{ "internalType": "uint256", "name": "", "type": "uint256" }], "stateMutability": "view", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "from", "type": "address" }, { "internalType": "address", "name": "to", "type": "address" }, { "internalType": "uint256", "name": "tokenId", "type": "uint256" }], "name": "transferFrom", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [{ "internalType": "address", "name": "newOwner", "type": "address" }], "name": "transferOwnership", "outputs": [], "stateMutability": "nonpayable", "type": "function" }, { "inputs": [], "name": "withDraw", "outputs": [], "stateMutability": "nonpayable", "type": "function" }]
