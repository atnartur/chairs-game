import MK from 'matreshka';

export default class Ws extends MK.Object {
    constructor() {
        super();
        this.on('open close message error', console.log)
    }
    connect() {
        this.conn = new WebSocket(`ws://${location.host}/ws`);
        const self = this;
        window.ws = this;
        this.conn.onopen = function() {
            self.trigger('open');
        };
        this.conn.onclose = function(e) {
            self.trigger('close', e);
        };
        this.conn.onmessage = function(e) {
            self.trigger('message', e);
            const json = JSON.parse(e.data);
            console.log(json.name, json.data);
            self.trigger(json.name, json.data);
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