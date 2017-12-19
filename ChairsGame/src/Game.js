import MK from 'matreshka';
import Preview from './Preview';
import Messages from './Messages/Collection';

export default class Game extends MK.Object {
    constructor() {
        super();
        this
            .jset({
                session: {},
                isHide: true,
                messages: ['Сообщение', 222]
            })
            .bindNode({
                sandbox: '#game'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .instantiate('preview', Preview)
            .instantiate('messages', Messages);
        console.log('game', this);
    }
    init(session) {
        this.session = session;
        this.isHide = false;
    }
    start() {
        this.preview.hide();
    }
}