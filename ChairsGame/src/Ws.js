import MK from 'matreshka';

export default class Ws extends MK.Object {
    constructor() {
        super();
        this.on('open close message error', console.log)
    }
    connect() {
        this.conn = new WebSocket(`ws://${location.host}/ws`);
        const self = this;
        this.conn.onopen = function() {
            self.trigger('open');
        };
        this.conn.onclose = function(e) {
            self.trigger('close', e);
        };
        this.conn.onmessage = function(e) {
            self.trigger('message', e);
        };
        this.conn.onerror = function(e) {
            self.trigger('error', e);
        };
        return new Promise((resolve, reject) => this.once('open', resolve));
    }
    send(name, data) {
        this.conn.send(JSON.stringify({name, data}));
    }
}