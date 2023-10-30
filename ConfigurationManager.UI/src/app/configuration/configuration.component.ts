import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { ConfigurationService } from './configuration.service';
import { ConfigurationModel } from './configuration.model';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css']
})
export class ConfigurationListComponent implements OnInit {
  constructor(
    private modalService: NgbModal, 
    private configurationService: ConfigurationService) { 

  }

  @ViewChild('editConfigurationModal') editConfigurationModal: any;

  configurations = new BehaviorSubject<ConfigurationModel[]>([]);
  allConfigurations: ConfigurationModel[] = [];
  editMode: boolean = false;
  configuration = new ConfigurationModel();
  searchText: string = '';

  ngOnInit(): void {
    this.loadConfigurations();
  }

  loadConfigurations(){
    this.configurationService.getItems().subscribe(items => {
      this.allConfigurations = items;
      this.configurations.next(items);
    });
  }

  onSearchInputChange() {
    let filteredItems = this.allConfigurations.filter(item => item.name.toLowerCase().includes(this.searchText.toLowerCase()));

    this.configurations.next(filteredItems);
  }

  onSave(){
    if(this.editMode){
      this.configurationService.updateItem(this.configuration).subscribe(() => {
        this.loadConfigurations();
      });
    }
    else{
      this.configurationService.createItem(this.configuration).subscribe(() => {
        this.loadConfigurations();
      });
    }
  }

  onEdit(configuration: ConfigurationModel){
    this.editMode = true;
    this.configuration = {...configuration};
    this.modalService.open(this.editConfigurationModal, { centered: true });
  }

  onCreate(){
    this.editMode = false;
    this.configuration = new ConfigurationModel();
    this.modalService.open(this.editConfigurationModal, { centered: true });
  }
}
