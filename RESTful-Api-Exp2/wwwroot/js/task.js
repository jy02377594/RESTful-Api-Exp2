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
