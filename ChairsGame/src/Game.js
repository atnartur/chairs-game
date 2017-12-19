import MK from 'matreshka';
import Preview from './Preview';
import Messages from './Messages/Collection';
import Ws from './Ws';
import Chairs from './Chairs/Collection'

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
            .calc('login', ['session.domain', 'session.uid'], (domain, uid) => domain ? domain : `id${uid}`)
            .bindNode('fullUserName', ':sandbox #username', MK.binders.text())
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .instantiate('preview', Preview)
            .instantiate('messages', Messages)
            .instantiate('ws', Ws)
            .instantiate('chairs', Chairs);
        console.log('game', this);
    }
    init(session) {
        this.session = session;
        this.isHide = false;   
        setTimeout(() => this.ws.connect().then(() => this.ws.send('login', {username: this.login})));
    }
    start() {
        this.preview.hide();
        this.chairs.show();
        this.chairs.setPositions(5);
    }
}