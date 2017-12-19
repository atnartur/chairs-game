import MK from 'matreshka';
import Model from './Model';

export default class Messages extends MK.Array {
    get Model() {
        return Model;
    }
    get itemRenderer() {
        return '#messageTemplate';
    }
    constructor(data) {
        super(...data);
        this
            .bindNode({
                sandbox: '#messagesList',
                container: ":sandbox"
            })
            .recreate(data);
    }
}