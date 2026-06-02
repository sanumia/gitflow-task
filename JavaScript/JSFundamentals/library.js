function isArray(obj) {
    return Array.isArray(obj);
}

function isBoolean(obj) {
    return typeof obj === 'boolean';
}

function isDate(obj) {
    return obj instanceof Date;
}

function isNumber(obj) {
    return typeof obj === 'number' && !isNaN(obj);
}

function isString(obj) {
    return typeof obj === 'string';
}

function isFunction(obj) {
    return typeof obj === 'function';
}

function isUndefined(obj) {
    return typeof obj === 'undefined';
}

function isNull(obj) {
    return obj === null;
}

function skip(arr, n) {
    return arr.slice(n);
}

function take(arr, n) {
    return arr.slice(0, n);
}

function asChain(arr) {
    return {
        _arr: arr,

        skip: function (n) {
            this._arr = skip(this._arr, n);
            return this;
        },

        take: function (n) {
            this._arr = take(this._arr, n);
            return this;
        },

        value: function () {
            return this._arr;
        }
    };
}

console.log(isArray([1, 2, 3]));
console.log(isBoolean(false));
console.log(isDate(new Date()));
console.log(isNumber(1));
console.log(isNumber(NaN));
console.log(isString('false'));
console.log(isFunction(() => { }));
console.log(isUndefined(undefined));
console.log(isNull(null));

console.log(asChain([1, 2, 3, 4, 5]).skip(1).take(3).value());