// FROM: http://helephant.com/2007/05/diy-javascript-stack-trace

function logError(ex, stack) {
    if (!ex) return;
    if (!window.logErrorUrl) {
        console.error('logErrorUrl must be defined.');
        return;
    }

    var url = ex.fileName ? ex.fileName : document.location;
    if (!stack && ex.stack) stack = ex.stack;

    // format output
    var out = ex.message ? ex.name + ': ' + ex.message : ex;
    out += `: at document path '${url}'.`;
    if (stack) out += `\n  at ${stack.join('\n  at ')}`;

    // send error message
    $.ajax({
        type: 'POST',
        url: window.logErrorUrl,
        data: { message: out }
    });
}

Function.prototype.trace = function () {
    var trace = [];
    var current = this;
    while (current) {
        trace.push(current.signature());
        current = current.caller;
    }
    return trace;
};

Function.prototype.signature = function () {
    var signature = {
        name: this.getName(),
        params: [],
        toString: function () {
            var params = this.params.length > 0 ?
                "'" + this.params.join("', '") + "'" : '';
            return this.name + '(' + params + ')'
        }
    };
    if (this.arguments) {
        for (let x = 0; x < this.arguments.length; x++)
            signature.params.push(this.arguments[x]);
    }

    return signature;
};

Function.prototype.getName = function () {
    if (this.name)
        return this.name;
    var definition = this.toString().split('\n')[0];
    var exp = /^function ([^\s(]+).+/;
    if (exp.test(definition))
        return definition.split('\n')[0].replace(exp, '$1') || 'anonymous';
    return 'anonymous';
};

window.onerror = function (msg, url, line) {
    let isGoogle = /google/i.test(navigator.userAgent);
    if (!isGoogle) {
        logError(msg, arguments.callee.trace());
    }
};
