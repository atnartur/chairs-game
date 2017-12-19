import MK from 'matreshka';
import Preview from './Preview';
import Messages from './Messages/Collection';
import Ws from './Ws';

export default class Game extends MK.Object {
    constructor() {
        super();
        this
            .jset({
                session: {},
                ws: {},
                isHide: true,
                messages: ['Сообщение', 222]
            })
            .bindNode({
                sandbox: '#game',
                fullUserName: ':sandbox #username'
            })
            .calc(
                'fullUserName',
                ['session.first_name', 'session.last_name'],
                (fn, ln) => fn ? `${fn} ${ln}` : 'Загрузка...'
            )
            .bindNode('fullUserName', ':sandbox #username', MK.binders.text())
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .instantiate('preview', Preview)
            .instantiate('messages', Messages)
            .instantiate('ws', Ws);
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