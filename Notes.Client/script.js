//////////////////////////////
// HTTP Utilities

let username = sessionStorage.getItem('username') || '';
let password = sessionStorage.getItem('password') || '';
const authorization = () => btoa(username + ":" + password);

const getText = (url, defaultValue, success) => {
    var request = new XMLHttpRequest();
    request.open('GET', url, true);
    request.setRequestHeader('Authorization','Basic ' + authorization());
    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(request.response);
        } else {
            console.log(`Call to ${url} failed`);
            alert('Call to Notes.Api failed');
            success(defaultValue);
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
        alert('Call to Notes.Api failed');
        success(defaultValue);
    };

    request.send();
};

const get = (url, defaultValue, success) => getText(url, defaultValue, (json) => success(JSON.parse(json)));

const post = (url, data, success, error) => {
    var request = new XMLHttpRequest();
    request.open('POST', url, true);
    request.setRequestHeader('Authorization','Basic ' + authorization());
    request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(JSON.parse(request.response));
        } else if (error) {
            error(request.response);
        } else {
            console.log(`Call to ${url} failed`);
            alert('Call to Notes.Api failed');
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
        alert('Call to Notes.Api failed');
    };

    request.send(JSON.stringify(data));
};

const patch = (url, data, success) => {
    var request = new XMLHttpRequest();
    request.open('PATCH', url, true);
    request.setRequestHeader('Authorization','Basic ' + authorization());
    request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(JSON.parse(request.response));
        } else {
            console.log(`Call to ${url} failed`);
            alert('Call to Notes.Api failed');
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
        alert('Call to Notes.Api failed');
    };

    request.send(JSON.stringify(data));
};

const remove = (url, success) => {
    var request = new XMLHttpRequest();
    request.open('DELETE', url, true);
    request.setRequestHeader('Authorization','Basic ' + authorization());

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success();
        } else {
            console.log(`Call to ${url} failed`);
            alert('Call to Notes.Api failed');
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
        alert('Call to Notes.Api failed');
    };

    request.send();
};

//////////////////////////////
// API Client

let url = 'http://localhost:5000';
if (window.location.href.includes('ngrok')) {
    url = 'https://sticky-notes.eu.ngrok.io';
}

const ping = () => getText(`${url}/ping`, 'NOT PONG', console.log);

const getNotes = (containing, success) => get(`${url}/notes?containing=${containing}`, '[]', success);

const createNote = (note, success) => post(`${url}/notes`, note, success);

const getNote = (noteId, success) => get(`${url}/notes/${noteId}`, '{}', success);

const updateNote = (noteId, content, success) => patch(`${url}/notes/${noteId}`, { content: content }, success);

const deleteNote = (noteId, success) => remove(`${url}/notes/${noteId}`, success);

const getAnswers = (success) => get(`${url}/answers`, '[]', success);

const submitAnswer = (answer, success, error) => post(`${url}/answers`, answer, success, error);

//////////////////////////////
// Menus and Dialogues

const loginDialogue = document.getElementById('login');

document
    .getElementById('openLogin')
    .addEventListener('click', () => loginDialogue.open = true);

document
    .getElementById('submitLogin')
    .addEventListener('click', () => {
        username = document.getElementById('username').value;
        sessionStorage.setItem('username', username);

        password = document.getElementById('password').value;
        sessionStorage.setItem('password', password);

        drawAllNotes(filter);
    });

let filter = '';
const filterDialogue = document.getElementById('filter');

document
    .getElementById('openFilter')
    .addEventListener('click', () => filterDialogue.open = true);

document
    .getElementById('submitFilter')
    .addEventListener('click', () => {
        filter = document.getElementById('containing').value;
        drawAllNotes(filter);
    });

const answerDialogue = document.getElementById('answer');
const correctAnswer = document.getElementById("correctAnswer");
const wrongAnswer = document.getElementById("wrongAnswer");

document
    .getElementById('openAnswer')
    .addEventListener('click', () => {
        wrongAnswer.hidden = true;
        correctAnswer.hidden = true;
        answerDialogue.open = true;
    });

document
    .getElementById('closeAnswer')
    .addEventListener('click', () => answerDialogue.open = false);

document
    .getElementById('submitAnswer')
    .addEventListener('click', () => {
        const flag = document.getElementById('flag');
        const name = document.getElementById('name');
        const solution = document.getElementById('solution');
        const answer = { flag: flag.value, name: name.value, solution: solution.value };

        submitAnswer(answer,
            () => {
                wrongAnswer.hidden = true;
                correctAnswer.hidden = false;
                solution.value = '';
            },
            () => {
                wrongAnswer.hidden = false;
                correctAnswer.hidden = true;
                solution.value = '';
            });
    });

const hackersDialogue = document.getElementById('hackers');
const topHackers = document.getElementById("topHackers");
const answersByName = answers =>
    answers.reduce((acc, pair) => {
        const current = acc.find(answer => answer.name === pair.name);
        if (current) {
            current.flags.push(pair.flag);
        } else {
            acc.push({
                name: pair.name,
                flags: [pair.flag]
            });
        }
        return acc;
    }, []);

const updateTopHackers = answers => {
    const updatedAnswers = answers.filter(a => a.name);
    const byName = answersByName(updatedAnswers);
    const sorted = byName.sort((a, b) => b.flags.length - a.flags.length);

    topHackers.textContent = "";
    sorted.forEach(answer => appendTopHacker(answer));

    return answers;
};

const appendTopHacker = answer => {
    const row = document.createElement("tr");
    const name = document.createElement("td");
    name.innerText = answer.name;

    const flagsFound = document.createElement("td");
    flagsFound.innerText = answer.flags.join(", ");

    const progress = document.createElement("td");
    progress.innerText = `${answer.flags.length}/6`;

    row.appendChild(name);
    row.appendChild(flagsFound);
    row.appendChild(progress);

    topHackers.appendChild(row);
};

document
    .getElementById('openHackers')
    .addEventListener('click', () =>
        getAnswers((answers) => {
            updateTopHackers(answers);
            hackersDialogue.open = true;
        }));

document
    .getElementById('closeHackers')
    .addEventListener('click', () => hackersDialogue.open = false);

//////////////////////////////
// Rendering Notes

const notesList = document.getElementById('notesList');

const noteListItem = note => {
    var content = document.createElement('div');
    content.setAttribute('class', 'note-content');
    content.innerHTML = note.content;

    var button = document.createElement('button');
    button.setAttribute('type', 'button');
    button.textContent = 'X';
    button.addEventListener('click', () =>
        deleteNote(
            note.id,
            () => drawAllNotes(filter)), false);

    var item = document.createElement('li');
    item.appendChild(content);
    item.appendChild(button);

    return item;
}

const newNoteListItem = filter => {
    var textarea = document.createElement('textarea');

    var button = document.createElement('button');
    button.setAttribute('type', 'button');
    button.textContent = '✓';
    button.addEventListener('click', () =>
        createNote(
            { content: textarea.value },
            () => drawAllNotes(filter)), false);

    var item = document.createElement('li');
    item.appendChild(textarea);
    item.appendChild(button);

    return item;
}

const drawAllNotes = filter =>
    getNotes(filter, notes => {
        const listItems = notes.map(note => noteListItem(note));
        listItems.push(newNoteListItem(filter));

        notesList.innerHTML = null;
        listItems.forEach(item => {
            notesList.appendChild(item);
        })
    });

if (password) {
    drawAllNotes(filter);
} else {
    loginDialogue.open = true;
}