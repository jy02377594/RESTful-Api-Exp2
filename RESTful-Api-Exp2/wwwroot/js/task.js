const uri = 'api/employeetask'
let todos = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(Object.values({ data })))
        .catch(error => console.error('Unable to get data.', error));
}

function getEmployees() {
    fetch('api/employees')
        .then(response => response.json())
        .then(data => _displayItems(Object.values({ data })))
        .catch(error => console.error('Unable to get data.', error));
}

function GetTasksByEmployeeId() {
    const employeeId = document.getElementById('employeeId').value;
    fetch(`${uri}/employee/${employeeId}`)
        .then(response => response.json())
        .then(data => data.status !== undefined ? alert(data.status + data.title) : _displayItems(Object.values({ data })))
        .catch(error => console.error('Unable to get data.', error));
}


function GetTaskByTaskName() {
    const taskName = document.getElementById('Search_taskName').value;
    fetch(`${uri}/SelectByTaskName?TaskName=${taskName}`)
        .then(response => response.json())
        .then(data => data.status !== undefined ? alert(data.status + data.title) : _displayItems(Object.values({ data }))) //有时候传回来的data是对象，有时候是数组，下面的displayItems是按数组处理的，所以这里要转换成数组
        .catch(error => console.error('Unable to get data.', error));
}

function GetTaskByTaskId() {
    const taskId = document.getElementById('taskId').value;
    fetch(`${uri}/${taskId}`)
        .then(response => response.json())
        .then(data => data.status !== undefined ? alert(data.status + data.title) : _displayItems(Object.values({ data })))//传回来如果只有一个数据就是object，多个就是array,按array处理
        .catch(error => console.error('Unable to get data.', error));
}

function GetTaskByTaskList() {
    const list = document.getElementById('taskList').value;
    fetch(`${uri}/SelectByTaskList?TaskList=${list}`)
        .then(response => response.json())
        .then(data => data.status !== undefined ? alert(data.status + data.title) : _displayItems(Object.values({ data }))) //有时候传回来的data是对象，有时候是数组，下面的displayItems是按数组处理的，所以这里要转换成数组
        .catch(error => console.error('Unable to get data.', error));
}

function GetTasksBySearch() {
    var deadline = document.getElementById('deadline').value;
    var dateFormat = /^(\d{4})-(\d{2})-(\d{2})$/;
    if (!dateFormat.test(deadline) && deadline != "") alert("The format of deadline is wrong");

	if (deadline != "") deadline = "deadline=" + deadline;
    const searchTerm = document.getElementById('searchTerm').value; 
    fetch(`${uri}/search?${deadline}&searchterm=${searchTerm}`)
        .then(response => response.json())
        .then(data => data.status !== undefined ? alert(data.status + data.title) : _displayItems(Object.values({ data }))) //有时候传回来的data是对象，有时候是数组，下面的displayItems是按数组处理的，所以这里要转换成数组
        .catch(error => console.error('Unable to get data.', error));
}

function AddTasks() {
    const employeeId = document.getElementById('employeeId').value; 
    const taskName = document.getElementById('taskName').value;
    const taskDescription = document.getElementById('taskDescription').value;
    const startTime = document.getElementById('startTime').value;
    const deadline = document.getElementById('deadline').value; 

    var dateFormat = /^(\d{4})-(\d{2})-(\d{2})$/;
    if (!dateFormat.test(deadline) && deadline != "") alert("The format of deadline is wrong");
    if (!dateFormat.test(startTime) && deadline != "") alert("The format of startTime is wrong");

    if (startTime > deadline) alert("time is wrong");
    /*
    //create a new taks but doesn't assign any employee
    if (employeeId == "") {
        var item = {
            taskName: taskName,
            taskDescription: taskDescription,
            startTime, startTime,
            Deadline, Deadline
        }

        fetch(`${uri}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(response => {
                if (!response.ok) alert('Unable to add item.' + response.status);
                else {
                    alert("Add successful");
                    getItems();
                }
            })
            .catch(error => console.log('Unable to add item.', error));
    }
    else { // create a new task and assign this taks to a employee
        var item = {
            employeeId: employeeId,
            taskName: taskName,
            taskDescription: taskDescription,
            startTime, startTime,
            Deadline, Deadline
        }

        fetch(`${uri}/employee/${employeeId}/CreateTaskWithEmployeeId`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(response => {
                if (!response.ok) alert('Unable to add item.' + response.status);
                else {
                    alert("Add successful");
                    getItems();
                }
            })
            .catch(error => console.log('Unable to add item.', error));

    }*/

    if (employeeId == "") {
        var item = {
            taskName: taskName,
            taskDescription: taskDescription,
            startTime, startTime,
            deadline, deadline
        }
        var uri2 = uri;
    }
    else {
        var item = {
            employeeId: employeeId,
            taskName: taskName,
            taskDescription: taskDescription,
            startTime, startTime,
            deadline, deadline
        }
        var uri2 = uri + "/employee/" + employeeId + "/CreateTaskWithEmployeeId";
    }

        fetch(`${uri2}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(response => {
                if (!response.ok) alert('Unable to add item.' + response.status);
                else {
                    alert("Add successful");
                    getItems();
                }
            })
            .catch(error => console.log('Unable to add item.', error));

}


function AddMultiTasks() {
    var itemArr = [];
    for (var i = 1; i <= 3; i++)
    {
        const taskName = document.getElementById('taskName' + i).value;
        const taskDescription = document.getElementById('taskDescription' + i).value;
        const startTime = document.getElementById('startTime' + i).value;
        const deadline = document.getElementById('deadline' + i).value;

        var dateFormat = /^(\d{4})-(\d{2})-(\d{2})$/;
        if (!dateFormat.test(deadline) && deadline != "") alert("The format of" + i + "th deadline is wrong");
        if (!dateFormat.test(startTime) && deadline != "") alert("The format of" + i + "th deadline is wrong");

        if (startTime > deadline) alert("time is wrong");

        var item = {
            taskName: taskName,
            taskDescription: taskDescription,
            startTime, startTime,
            deadline, deadline
        }

        if(item != null)
        itemArr.push(item);
    }

    fetch(`${uri}/taskcollection`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(itemArr)
    })
        .then(response => {
            if (!response.ok) alert('Unable to add item.' + response.status);
            else {
                alert("Add successful");
                getItems();
            }
        })
        .catch(error => console.log('Unable to add item.', error));
}



function updateItem() {
    const taskId = document.getElementById('update_task_id').value;
    const employeeId = document.getElementById('update_employeeId').value;
    const taskName = document.getElementById('update_taskName').value;
    const taskDescription = document.getElementById('update_taskDescription').value;
    const startTime = document.getElementById('update_startTime').value;
    const deadline = document.getElementById('update_deadline').value;
    if (taskId == "") alert("updating taskId is reqiured");

    var item_employeeId =
    {
        op: "replace",
        path: "/employeeId",
        value: employeeId   
    }

    var item_taskName =
    {
        op: "replace",
        path: "/taskName",
        value: taskName
    }

    var item_taskDescription =
    {
        op: "replace",
        path: "/taskDescription",
        value: taskDescription
    }

    var item_startTime =
    {
        op: "replace",
        path: "/startTime",
        value: startTime
    }

    var item_deadline =
    {
        op: "replace",
        path: "/deadline",
        value: deadline
    }

    var item = [];
    if (employeeId.trim() != '') { item.push(item_employeeId); }
    if (taskName.trim() != '') { item.push(item_taskName); }
    if (taskDescription.trim() != '') { item.push(item_taskDescription); }
    if (startTime.trim() != '') { item.push(item_startTime); }
    if (deadline.trim() != '') { item.push(item_deadline); }

    var body = JSON.stringify(item);
    if (body == []) alert("A non-empty request body is required");
    fetch(`${uri}/${taskId}`, {
        method: 'PATCH',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: body
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

function deleteItem() {
    var taskId = document.getElementById('deleteId').value;
    fetch(`${uri}/${taskId}`, {
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



function _displayItems(data) {
    let btn = document.querySelector('#json');
    btn.textContent = "";
    data.forEach(item => {
        btn.textContent += JSON.stringify(item, null, "  ");
    })
}



function JumpToEmployee() {
    window.location.href = 'indexl.html';
}
