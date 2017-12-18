import MK from 'matreshka';

export default class Game extends MK.Object {
    constructor() {
        super();
        this
            .jset({
                session: {},
                isHide: true
            })
            .bindNode({
                sandbox: '#game'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
    }
    start(session) {
        this.session = session;
        this.isHide = false;
    }
}