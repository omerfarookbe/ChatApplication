
const storage = window.localStorage;

window.getFromStorage = (key) => {
    return storage.getItem(key);
}

window.setToStorage = (key, value) => {
    storage.setItem(key, value);
}

window.removeFromStorage = (key) => {
    storage.removeItem(key);
}