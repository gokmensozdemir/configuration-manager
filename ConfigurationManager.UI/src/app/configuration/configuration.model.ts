export class ConfigurationModel{
    id: number;
    name: string;
    type: string;
    value: string;
    isActive: boolean;
    applicationName: string;
    constructor(){
        this.id=0;
        this.name = '';
        this.type = 'String';
        this.value = '';
        this.isActive = false;
        this.applicationName = ''
    }
}