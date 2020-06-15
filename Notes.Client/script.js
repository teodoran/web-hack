//////////////////////////////
// HTTP Utilities

const getText = (url, success) => {
    var request = new XMLHttpRequest();
    request.open('GET', url, true);
    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(request.response);
        } else {
            console.log(`Call to ${url} failed`);
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
    };

    request.send();
};

const get = (url, success) => getText(url, (json) => success(JSON.parse(json)));

const post = (url, data, success) => {
    var request = new XMLHttpRequest();
    request.open('POST', url, true);
    request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(JSON.parse(request.response));
        } else {
            console.log(`Call to ${url} failed`);
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
    };

    request.send(JSON.stringify(data));
};

const patch = (url, data, success) => {
    var request = new XMLHttpRequest();
    request.open('PATCH', url, true);
    request.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success(JSON.parse(request.response));
        } else {
            console.log(`Call to ${url} failed`);
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
    };

    request.send(JSON.stringify(data));
};

const remove = (url, success) => {
    var request = new XMLHttpRequest();
    request.open('DELETE', url, true);

    request.onload = () => {
        if (request.status >= 200 && request.status < 400) {
            success();
        } else {
            console.log(`Call to ${url} failed`);
        }
    };

    request.onerror = () => {
        console.log(`Call to ${url} failed`);
    };

    request.send();
};

//////////////////////////////
// API Client

let url = 'http://localhost:5000';
if (window.location.href.includes('stickynotes')) {
    url = 'https://sticky-notes-api.azurewebsites.net';
}
if (window.location.href.includes('stickynotestest')) {
    url = 'https://sticky-notes-api-test.azurewebsites.net';
}

const ping = () => getText(`${url}/ping`, console.log);

const getNotes = (author, success) => get(`${url}/notes?author=${author}`, success);

const createNote = (note, success) => post(`${url}/notes`, note, success);

const getNote = (noteId, success) => get(`${url}/notes/${noteId}`, success);

const updateNote = (noteId, content, success) => patch(`${url}/notes/${noteId}`, { content: content }, success);

const deleteNote = (noteId, success) => remove(`${url}/notes/${noteId}`, success);

//////////////////////////////
// Rendering Notes

const notesList = document.getElementById('notesList');

const author = window.location.search.substring(1) || 'default';

const noteListItem = note => {
    var textarea = document.createElement('textarea');
    textarea.value = note.content;
    textarea.addEventListener('change', () =>
        updateNote(
            note.id,
            textarea.value,
            updatedNote => {
                textarea.value = updatedNote.content;
            }), false);

    var button = document.createElement('button');
    button.setAttribute('type', 'button');
    button.textContent = 'X';
    button.addEventListener('click', () =>
        deleteNote(
            note.id,
            () => drawAllNotes(author)), false);

    var item = document.createElement('li');
    item.appendChild(textarea);
    item.appendChild(button);

    return item;
}

const newNoteListItem = author => {
    var textarea = document.createElement('textarea');

    var button = document.createElement('button');
    button.setAttribute('type', 'button');
    button.textContent = '✓';
    button.addEventListener('click', () =>
        createNote({
            author: author,
            content: textarea.value},
            () => drawAllNotes(author)), false);

    var item = document.createElement('li');
    item.appendChild(textarea);
    item.appendChild(button);

    return item;
}

const drawAllNotes = author =>
    getNotes(author, notes => {
        const listItems = notes.map(note => noteListItem(note));
        listItems.push(newNoteListItem(author));

        notesList.innerHTML = null;
        listItems.forEach(item => {
            notesList.appendChild(item);
        })
    });

drawAllNotes(author);