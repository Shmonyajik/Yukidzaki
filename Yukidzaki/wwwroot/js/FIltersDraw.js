var valueList = document.getElementById('valueList');
var text = '<span> Filters : </span>';
var listArray = [];
var checkboxes = document.querySelectorAll('.checkbox');

for (var checkbox of checkboxes) {
    checkbox.addEventListener('click', function () {
        if (this.checked == true) {
            listArray.push(this.value);
            valueList.innerHTML = text + listArray.join(' / ');
        }
        else {
            listArray = listArray.filter(e => e !== this.value);
            valueList.innerHTML = text + listArray.join(' / ');
        }
    })
}