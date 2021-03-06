import MK from 'matreshka'

export default class Preview extends MK.Object {
    constructor(data, parent) {
        super();
        this
            .jset({
                playersCount: 0,
                isHide: false,
                isStartButtonHide: true,
                text: 'Ожидаем подключения игроков'
            })
            .bindNode({
                sandbox: '#preview',
                startGameButton: ':sandbox #startGameButton'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .bindNode('isStartButtonHide', ':sandbox #startGameButton', MK.binders.display(false))
            .bindNode('playersCount', ':sandbox #playersCount', MK.binders.text())
            .bindNode('text', ':sandbox #previewText', MK.binders.text())
            .on('click::startGameButton', () => parent.start());
    }
}