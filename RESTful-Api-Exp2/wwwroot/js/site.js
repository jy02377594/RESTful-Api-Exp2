const uri = 'api/employees';
const taskuri = 'api/employeetask'
let todos = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getCompanyItems() {
    fetch('api/companies')
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get Company items.', error))
}

function getTaskItems() {
    fetch(taskuri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get Company items.', error))
}

function addItem() {
    const employeeNo = document.getElementById('employeeNo').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const hiredDate = document.getElementById('hiredDate').value;
    var gender = document.getElementById('gender').value;
    var genderRange = ["male", "female", "男", "女"];
    if (genderRange.includes(gender)) {
        if (gender == "male" || gender == "男") gender = 1;
        if (gender == "female" || gender == "女") gender = 2;
    }
    else window.alert("Your gender input is error");

    var item = {
        employeeNo: employeeNo,
        firstName: firstName,
        lastName: lastName,
        hiredDate: hiredDate,
        gender: gender,
    };

    const taskName = document.getElementById('taskName').value;
    const taskDescription = document.getElementById('taskDescription').value;
    const startTime = document.getElementById('startTime').value;
    const Deadline = document.getElementById('Deadline').value;
    if (taskName.trim() != "" || taskDescription.trim() != "" || startTime.trim() != "" || Deadline.trim() != "") {
        item = {
            employeeNo: employeeNo,
            firstName: firstName,
            lastName: lastName,
            hiredDate: hiredDate,
            gender: gender,
            Tasklist: [
                {
                    taskName: taskName,
                    taskDescription: taskDescription,
                    startTime: startTime,
                    Deadline: Deadline
                }
            ]
        };
    }

    var companyId = document.getElementById('companyId').value;
    if (companyId.trim() == "") alert("you can not add employee without company");

    fetch(`${uri}/${companyId}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            if (!response.ok) {
                alert('Unable to add item.' + response.status);
            }
            else {
                alert("Add successful");
                getItems();
            }
        })
        .catch(error => console.log('Unable to add item.', error));
}

function deleteItem() {
    var employeeid = document.getElementById('deleteId').value;
    fetch(`${uri}/${employeeid}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (!response.ok) {
                alert('Unable to delete item.' + response.status);
            }
            else {
                alert("delete successful");
                getItems()
            }
        })
        .catch(error => alert("Unable to delete item" + errors)/*error => console.error('Unable to delete item.', error)*/);
}


function updateItem() {
    const employeeId = document.getElementById('update_id').value;

    const employeeNo = document.getElementById('update_employeeNo').value;
    const firstName = document.getElementById('update_firstName').value;
    const lastName = document.getElementById('update_lastName').value;
    const hiredDate = document.getElementById('update_hiredDate').value;
    var gender = document.getElementById('update_gender').value;
    var genderRange = ["male", "female", "男", "女"];
    if (genderRange.includes(gender)) {
        if (gender == "male" || gender == "男") gender = 1;
        if (gender == "female" || gender == "女") gender = 2;
    }
    else window.alert("Your gender input is error");

    const item_employeeNo =
    {
        op: "replace",
        path: "/employeeNo",
        value: employeeNo
    }

    const item_firstName = {
        op: "replace",
        path: "/firstName",
        value: firstName
    }

    const item_lastName = {
        op: "replace",
        path: "/lastName",
        value: lastName
    }

    const item_hiredDate = {
        op: "replace",
        path: "/hiredDate",
        value: hiredDate
    }

    const item_gender = {
        op: "replace",
        path: "/gender",
        value: gender
    }

    var item = [];
    if (employeeNo.trim() != '') { item.push(item_employeeNo); }
    if (firstName.trim() != '') { item.push(item_firstName); }
    if (lastName.trim() != '') { item.push(item_lastName); }
    if (hiredDate.trim() != '') { item.push(item_hiredDate); }
    if (gender != null) { item.push(item_gender); }

    var a = JSON.stringify(item);
    if (a == []) {
        alert("A non - empty request body is required");
    }
    fetch(`${uri}/${employeeId}`, {
        method: 'PATCH',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            if (!response.ok) {
                alert('update fail.' + response.status);
            }
            else {
                alert("update successful");
                getItems()
            }
        })
        .catch(error => console.error('Unable to update item.', error));
}


function _displayItems(data) {
    let btn = document.querySelector('#json');
    btn.textContent = "";
    //for (var i = 0; i < data.length; i++) {
    //    btn.textContent += JSON.stringify(data[i], null, '  ');
    //}
    data.forEach(item => {
        btn.textContent += JSON.stringify(item, null, '  ');
    })
}

function _displayCompanyItems(data) {
    let btn = document.querySelector('#companyjson');
    btn.textContent = "";
    data.forEach(item => {
        btn.textContent += JSON.stringify(item, null, '  ');
    })
}