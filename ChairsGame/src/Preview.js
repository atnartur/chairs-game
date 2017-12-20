import MK from 'matreshka'

export default class Preview extends MK.Object {
    constructor(data, parent) {
        super();
        this
            .jset({
                playersCount: 0,
                isHide: false,
                isStartButtonHide: true
            })
            .bindNode({
                sandbox: '#preview',
                playersCount: ':sandbox #playersCount',
                startGameButton: ':sandbox #startGameButton'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .bindNode('isStartButtonHide', ':sandbox #startGameButton', MK.binders.display(false))
            .on('click::startGameButton', () => parent.start());
    }
}