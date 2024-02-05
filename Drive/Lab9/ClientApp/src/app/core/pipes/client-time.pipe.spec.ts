import { ClientTimePipe } from './client-time.pipe';

describe('ClientTimePipe', () => {
  it('create an instance', () => {
    const pipe = new ClientTimePipe();
    expect(pipe).toBeTruthy();
  });
});
