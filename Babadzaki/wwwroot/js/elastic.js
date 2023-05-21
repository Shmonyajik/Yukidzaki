
document.querySelector('#elastic_type').oninput = function () {
    let val = this.value.trim();
    let val1 = this.value.toLowerCase();
    let elasticItems = document.querySelectorAll('.elastic_type li');
    if (val != '') {
        elasticItems.forEach(function (elem) {
            if (elem.innerText.search(val) == -1) {
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
            }
        });
    }
    else {
        elasticItems.forEach(function (elem) {
            elem.classList.remove('hide');
        });
    }
}


document.querySelector('#elastic_clothigs').oninput = function () {
    let val = this.value.trim();
    let elasticItems = document.querySelectorAll('.elastic_clothigs li');
    if (val != '') {
        elasticItems.forEach(function (elem) {
            if (elem.innerText.search(val) == -1) {
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
            }
        });
    }
    else {
        elasticItems.forEach(function (elem) {
            elem.classList.remove('hide');
        });
    }
}

document.querySelector('#elastic_hair').oninput = function () {
    let val = this.value.trim();
    let elasticItems = document.querySelectorAll('.elastic_hair li');
    if (val != '') {
        elasticItems.forEach(function (elem) {
            if (elem.innerText.search(val) == -1) {
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
            }
        });
    }
    else {
        elasticItems.forEach(function (elem) {
            elem.classList.remove('hide');
        });
    }
}


document.querySelector('#elastic_clothigs').oninput = function () {
    let val = this.value.trim();
    let elasticItems = document.querySelectorAll('.elastic_clothigs li');
    if (val != '') {
        elasticItems.forEach(function (elem) {
            if (elem.innerText.search(val) == -1) {
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
            }
        });
    }
    else {
        elasticItems.forEach(function (elem) {
            elem.classList.remove('hide');
        });
    }
}


document.querySelector('#elastic_backgrounds').oninput = function () {
    let val = this.value.trim();
    let elasticItems = document.querySelectorAll('.elastic_backgrounds li');
    if (val != '') {
        elasticItems.forEach(function (elem) {
            if (elem.innerText.search(val) == -1) {
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
            }
        });
    }
    else {
        elasticItems.forEach(function (elem) {
            elem.classList.remove('hide');
        });
    }
}
