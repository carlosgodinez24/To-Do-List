import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Task } from '../../models/task.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss'],
})
export class TaskFormComponent implements OnInit {
  taskForm!: FormGroup;
  isEditMode = false;
  taskId!: string;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      dueDate: [null],
    });

    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.isEditMode = true;
        this.taskId = params['id'];
        this.loadTask();
      }
    });
  }

  loadTask() {
    this.taskService.getTaskById(this.taskId).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          dueDate: task.dueDate ? new Date(task.dueDate) : null,
        });
      },
      error: (error) => console.error(error),
    });
  }

  onSubmit() {
    if (this.taskForm.invalid) {
      return;
    }

    const taskData = this.taskForm.value as Task;

    if (this.isEditMode) {
      this.taskService.updateTask(this.taskId, taskData).subscribe({
        next: () => {
          this.snackBar.open('Task updated', 'Close', { duration: 2000 });
          this.router.navigate(['/tasks']);
        },
        error: (error) => console.error(error),
      });
    } else {
      this.taskService.createTask(taskData).subscribe({
        next: () => {
          this.snackBar.open('Task created', 'Close', { duration: 2000 });
          this.router.navigate(['/tasks']);
        },
        error: (error) => console.error(error),
      });
    }
  }

  goBack() {
    this.router.navigate(['/tasks']);
  }
}
