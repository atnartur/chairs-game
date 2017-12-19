import MK from 'matreshka';

export default class Message extends MK.Object {
    constructor(data) {
        super({text: data});
        this.on('render', () => this.bindNode('text', ':sandbox', MK.binders.text()));
    }
}